using Microsoft.EntityFrameworkCore;
using ShopNow.DAL.DomainClasses;

namespace ShopNow.DAL.DAO
{
    public class CustomerDAO
    {

        private AppDbContext _db;
        public CustomerDAO(AppDbContext ctx)
        {
            _db = ctx;
        }

        public async Task<Customer> Register(Customer Customer)
        {
            await _db.Customers.AddAsync(Customer);
            await _db.SaveChangesAsync();
            return Customer;
        }

        public async Task<Customer> GetByEmail(string email)
        {
            //get the first or default result after finding matching email
            Customer Customer = await _db.Customers.FirstOrDefaultAsync(x => x.Email == email);
            return Customer;
        }
    }
}
