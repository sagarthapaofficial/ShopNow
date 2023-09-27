namespace ShopNow.Helpers
{

    public class OrderDetailsHelper
    {
        //properties that will be needed to show in the dialog for the orderDetails
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public string Name { get; set; }
        public int CustomerId { get; set; }
        public string DateCreated { get; set; }
        public int QtyO { get; set; } //Quantity Ordered
        public int QtyS { get; set; } //Quantity sold
        public int QtyB { get; set; } //Quantity back ordered
        public decimal sellingPrice { get; set; }


    }
}
