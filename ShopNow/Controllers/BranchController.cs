using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopNow.DAL;
using ShopNow.DAL.DAO;
using ShopNow.DAL.DomainClasses;

namespace ShopNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchController : ControllerBase
    {
        AppDbContext _db;
        public BranchController(AppDbContext context)
        {
            _db = context;
        }

        // provides the list of Branch to the client
        [AllowAnonymous]
        [HttpGet("{lat}/{lon}")]
        public async Task<ActionResult<List<Branch>>> Index(float lat, float lon)
        {
            BranchDAO dao = new BranchDAO(_db);
            return await dao.GetThreeClosestBranchs(lat, lon);
        }


    }
}
