using Microsoft.AspNetCore.Mvc;
using ShopNow.DAL;
using ShopNow.DAL.DAO;
using ShopNow.DAL.DomainClasses;

namespace ShopNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class StoreController : ControllerBase
    {
        AppDbContext _db;
        public StoreController(AppDbContext context)
        {
            _db = context;
        }

        // provides the list of store to the client
        [HttpGet("{lat}/{lon}")]
        public async Task<ActionResult<List<Store>>> Index(float lat, float lon)
        {
            StoreDAO dao = new StoreDAO(_db);
            return await dao.GetThreeClosestStores(lat, lon);
        }
    }
}
