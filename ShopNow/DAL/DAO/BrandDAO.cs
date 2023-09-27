using Microsoft.EntityFrameworkCore;
using ShopNow.DAL.DomainClasses;

namespace ShopNow.DAL.DAO
{
    public class BrandDAO
    {

        private AppDbContext _db;
        public BrandDAO(AppDbContext ctx)
        {
            _db = ctx;
        }

        public async Task<List<Brand>> GetAll()
        {
            //returns the list of brands.
            //use of context class to return all the brands on db
            return await _db.Brands.ToListAsync<Brand>();
        }


    }
}
