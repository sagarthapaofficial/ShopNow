using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShopNow.DAL.DomainClasses;

namespace ShopNow.DAL.DAO
{
    public class BranchDAO
    {

        private AppDbContext _db;

        public BranchDAO(AppDbContext context)
        {
            _db = context;
        }


        //get the 3 closest branch 

        //gets the list of 3 closest Branch based on distance
        public async Task<List<Branch>> GetThreeClosestBranchs(float? lat, float? lon)
        {
            List<Branch> BranchDetails = null;

            try
            {
                var latParam = new SqlParameter("@lat", lat);
                var lonParam = new SqlParameter("@lon", lon);

                //passing the longtitude and latitude to Branchprocedure.
                var query = _db.Branches.FromSqlRaw("dbo.pGetThreeClosestBranches @lat, @lon", latParam,
                lonParam);

                // converts the result comming from query to list.
                BranchDetails = await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BranchDetails;
        }

    }
}
