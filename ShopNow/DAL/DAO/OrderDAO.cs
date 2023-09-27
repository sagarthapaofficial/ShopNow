using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopNow.DAL.DomainClasses;
using ShopNow.Helpers;

namespace ShopNow.DAL.DAO
{
    public class OrderDAO
    {
        private AppDbContext _db;
       
        public OrderDAO(AppDbContext ctxb)
        {
            this._db = ctxb;
        }




        //Adds the order to the database.
        public async Task<int>AddOrder(int customerId, OrderSelectionHelper[] selections)
        {
            int orderId = -1;
            using (_db)
            {
                //we need a transaction as multiple entities involved.
                using(var _trans= await _db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Order order = new Order();
                        order.UserId = customerId;
                        order.OrderDate = DateTime.Now;
                        order.OrderAmount = 0;

                        //add order row and 
                        foreach(OrderSelectionHelper selection in selections)
                        {
                            order.OrderAmount += selection.qty * selection.product.MSRP;
                        }

                        //add the data to the order table.
                        await _db.Orders.AddAsync(order);
                        await _db.SaveChangesAsync();

                        //subsequently a series of order Line item row to the database.

                        //let say we have 5 kinds of product.
                        foreach(OrderSelectionHelper selection in selections)
                        {
                            OrderLineItem lineItem = new OrderLineItem();
                            lineItem.OrderId = order.Id;
                            lineItem.ProductId = selection.product.Id;


                            lineItem.QtyOrdered= selection.qty;
                           
                          
                            lineItem.SellingPrice= selection.product.MSRP;
                            

                            //if QtyOrdered is greater than qty on hand on database.
                            if (lineItem.QtyOrdered>selection.product.QtyOnHand)
                            {

                                selection.product.QtyOnHand = 0;
                                selection.product.QtyOnBackOrder += selection.qty - selection.product.QtyOnHand;
                                lineItem.QtyOrdered = selection.qty;
                                lineItem.QtySold = selection.product.QtyOnHand;
                                lineItem.QtyBackOrdered = selection.qty - selection.product.QtyOnHand;


                            }
                            else
                            {
                                lineItem.QtySold = lineItem.QtyOrdered;
                                selection.product.QtyOnHand -= selection.qty;
                                lineItem.QtyBackOrdered = 0;

                            }


                            // sends to get the produc to be updated
                            ProductDAO pdao = new ProductDAO(_db);

                            Product product = await pdao.GetById(selection.product.Id);
                            product.QtyOnHand = selection.product.QtyOnHand;

                            //Saving the data in the table.

                            await _db.OrderLineItems.AddAsync(lineItem);
                            await _db.SaveChangesAsync();
                        }
                        await _trans.CommitAsync();
                        orderId = order.Id;


                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await _trans.RollbackAsync();
                    }


                }
            }
            return orderId;
        }


        //list of order based on customer ID.
        public async Task<List<Order>> GetAll(int id)
        {
            return await _db.Orders.Where(Order => Order.UserId == id).ToListAsync<Order>();
        }

        //get the details of the order.
        public async Task<List<OrderDetailsHelper>>GetOrderDetails(int oId, string email)
        {
            Customer cus = _db.Customers.FirstOrDefault(customer => customer.Email == email);

            //holds list of order Details.
            List<OrderDetailsHelper> allDedtails = new List<OrderDetailsHelper>();

            //LINQ way of doing inner joins
            var results = from o in _db.Orders
                          join oi in _db.OrderLineItems on o.Id equals oi.OrderId
                          join pi in _db.Products on oi.ProductId equals pi.Id
                          where (o.UserId == cus.Id && o.Id == oId)
                          select new OrderDetailsHelper
                          {
                              OrderId = o.Id,
                              CustomerId = cus.Id,
                              ProductId = oi.Id,
                              Name=pi.ProductName,
                              DateCreated = o.OrderDate.ToString("yyyy/MM/dd - hh:mm tt"),  //formats the date to this
                              QtyO = oi.QtyOrdered,
                              QtyS = oi.QtySold,
                              QtyB = oi.QtyBackOrdered,
                              sellingPrice=oi.SellingPrice

                          };

            //gets all the order list into this ORderListDetails.
            allDedtails= await results.ToListAsync<OrderDetailsHelper>();


            return allDedtails;

        }




    }
}
