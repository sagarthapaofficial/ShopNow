namespace ShopNow.Helpers
{
    public class CustomerHelper
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        //This will be generated JWT
        public string? token { get; set; }
    }
}
