using ShopNow.DAL.DomainClasses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShopNow.DAL;


namespace ShopNow.DAL.DAO
{
    public class StoreDAO
    {

        private AppDbContext _db;

        public StoreDAO(AppDbContext context)
        {
            _db = context;
        }

        public async Task<bool> LoadStoresFromFile(string path)
        {
            bool addWorked = false;
            try
            {
                // clear out the old rows
                _db.Stores.RemoveRange(_db.Stores);
                await _db.SaveChangesAsync();
                var csv = new List<string[]>();
                var csvFile = path + "\\exercisesStoreRaw.csv";

                //Reads all the line from the csv file.
                var lines = await System.IO.File.ReadAllLinesAsync(csvFile);
                
                
                //add each word to the csv list
                foreach (string line in lines)
                    csv.Add(line.Split(',')); // populate store with csv
               
                
                foreach (string[] rawdata in csv)
                {
                    Store aStore = new Store();
                    aStore.Longitude = Convert.ToDouble(rawdata[0]);
                    aStore.Latitude = Convert.ToDouble(rawdata[1]);
                    aStore.Street = rawdata[2];
                    aStore.City = rawdata[3];
                    aStore.Region = rawdata[4];
                    await _db.Stores.AddAsync(aStore);
                    await _db.SaveChangesAsync();
                }
                addWorked = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return addWorked;
        }

        //gets the list of 3 closest store based on distance
        public async Task<List<Store>> GetThreeClosestStores(float? lat, float? lon)
        {
            List<Store> storeDetails = null;

            try
            {
                var latParam = new SqlParameter("@lat", lat);
                var lonParam = new SqlParameter("@lon", lon);

                //passing the longtitude and latitude to storeprocedure.
                var query = _db.Stores.FromSqlRaw("dbo.pGetThreeClosestStores @lat, @lon", latParam,
                lonParam);

                // converts the result comming from query to list.
                storeDetails = await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return storeDetails;
        }


    }
}
