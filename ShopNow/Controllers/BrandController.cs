using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopNow.DAL;
using ShopNow.DAL.DAO;
using ShopNow.DAL.DomainClasses;

namespace ShopNow.Controllers
{
    //will not allow user to do requests
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {

        AppDbContext _db;
        public BrandController(AppDbContext context)
        {
            _db = context;
        }

        //it will return the list of brand when called the \brand
        public async Task<ActionResult<List<Brand>>> Index()
        {
            BrandDAO dao = new BrandDAO(_db);
            List<Brand> allCategories = await dao.GetAll();
            return allCategories;
        }

    }
}
