using Microsoft.EntityFrameworkCore;
using ShopNow.DAL.DomainClasses;

namespace ShopNow.DAL.DAO
{
    public class ProductDAO
    {
        private AppDbContext _db;
        public ProductDAO(AppDbContext ctx)
        {
            _db = ctx;
        }

        //retrieve the Product list based on brandID.
        public async Task<List<Product>>GetAllByBrand(int brandID)
        {
            return await _db.Products.Where(item => item.Brand.Id == brandID).ToListAsync();
        }

        // returns the product base on ID
        public async Task<Product> GetById(string id)
        {
            Product p = await _db.Products.FirstOrDefaultAsync(u => u.Id == id);
            return p;
        }

    }
}
