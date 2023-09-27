using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopNow.DAL;
using ShopNow.DAL.DAO;
using ShopNow.DAL.DomainClasses;
using ShopNow.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        AppDbContext _ctx;
        public OrderController(AppDbContext context) // injected here
        {
            this._ctx = context;
        }



        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<string>> Index(OrderHelper helper)
        {

            string retVal = "";
            CustomerDAO dao = new CustomerDAO(_ctx);
            Customer customer = await dao.GetByEmail(helper.email);

            OrderDAO orderDAO = new OrderDAO(_ctx);
            bool isBackOrdered = false;



            try
            {

                ProductDAO prodDAO = new ProductDAO(_ctx);

                foreach (OrderSelectionHelper p in helper.Selections)
                {
                    Product product = await prodDAO.GetById(p.product.Id);


                    // sets isBbackordered to true if the quantity on hand is less
                    // than the quantity choosen by the customer
                    if (product.QtyOnHand < p.qty)
                    {
                        isBackOrdered = true;
                    }
                }

                int OrderId = await orderDAO.AddOrder(customer.Id, helper.Selections);

                if (OrderId > 0)
                {
                    // sends the message if the backordered is true
                    if (isBackOrdered)
                    {
                        retVal = "Order " + OrderId + " Created! Goods Backordered";

                    }
                    else
                    {
                        retVal = "Order " + OrderId + " Created!";
                    }

                }
                else
                {
                    retVal = "Order not Created";
                }

            }
            catch (Exception ex)
            {
                retVal = $"Oder not saved {ex.Message}";
            }

            return retVal;
        }


        //return the list of order for a particular customer.

        [Route("{email}")]
        public async Task<ActionResult<List<Order>>> List(string email)
        {
            List<Order> Orders = new List<Order>();
            CustomerDAO uDao = new CustomerDAO(_ctx);
            Customer OrderOwner = await uDao.GetByEmail(email);
            OrderDAO tDao = new OrderDAO(_ctx);
            Orders = await tDao.GetAll(OrderOwner.Id);
            return Orders;
        }

        //meed to get the orderDetails
        [Route("{orderid}/{email}")] //send multiple items to the controller.
        public async Task<ActionResult<List<OrderDetailsHelper>>>GetOrderDetails(int orderid, string email)
        {
            //initialize orderDAO
            OrderDAO tDao = new OrderDAO(_ctx);
            //gets the list from helper method
            return await tDao.GetOrderDetails(orderid, email);

        }



    }
}
