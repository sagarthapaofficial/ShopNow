using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopNow.DAL;
using ShopNow.DAL.DAO;
using ShopNow.DAL.DomainClasses;

namespace ShopNow.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        AppDbContext _db;
        public ProductController(AppDbContext context)
        {
            _db = context;
        }

        //we passs the brand ID to get list of all product by brand

        [Route("{catid}")]
        public async Task<ActionResult<List<Product>>> Index(int catid)
        {
            //passing the db context to the ProductDAO
            ProductDAO dao = new ProductDAO(_db);
            List<Product> itemsForCategory = await dao.GetAllByBrand(catid);
            return itemsForCategory;
        }
    }
}
