using ShopNow.DAL.DomainClasses;
using System.Text.Json;

namespace ShopNow.DAL
{
    public class DataUtility
    {
        private AppDbContext _db;
        //dependency injection is done here
        public DataUtility(AppDbContext context)
        {
            _db = context;
        }

        public async Task<bool> loadProductInfoFromWebToDb(string stringJson)
        {
            bool BrandsLoaded = false;
            bool ProductsLoaded = false;
            try
            {
                // an element that is typed as dynamic is assumed to support any operation
                dynamic objectJson = JsonSerializer.Deserialize<Object>(stringJson);
                BrandsLoaded = await loadBrands(objectJson);
                ProductsLoaded = await loadProduct(objectJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BrandsLoaded && ProductsLoaded;
        }


        private async Task<bool> loadBrands(dynamic jsonObjectArray)
        {
            bool loadedBrands = false;
            try
            {
                // clear out the old rows
                _db.Brands.RemoveRange(_db.Brands);
                await _db.SaveChangesAsync();
                List<String> allBrands = new List<String>();
                foreach (JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    if (element.TryGetProperty("BrandID", out JsonElement brandJson))
                    {
                        allBrands.Add(brandJson.GetString());
                    }
                }
                IEnumerable<String> Brands = allBrands.Distinct<String>();
                foreach (string catname in Brands)
                {
                    Brand cat = new Brand();
                    cat.Name = catname;
                    await _db.Brands.AddAsync(cat);
                    await _db.SaveChangesAsync();
                }
                loadedBrands = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedBrands;
        }

        private async Task<bool> loadProduct(dynamic jsonObjectArray)
        {
            bool loadedItems = false;
            try
            {
                List<Brand> Brands = _db.Brands.ToList();
                // clear out the old
                _db.Products.RemoveRange(_db.Products);
                await _db.SaveChangesAsync();

                foreach (JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    Product item = new Product();
                    item.Id = element.GetProperty("ID").GetString();

                    item.ProductName = element.GetProperty("ProductName").GetString();

                    item.GraphicName = element.GetProperty("GraphicName").GetString();

                    item.CostPrice = Convert.ToDecimal(element.GetProperty("costPrice").GetString());

                    item.MSRP = Convert.ToDecimal(element.GetProperty("MSRP").GetString());

                    item.QtyOnHand = Convert.ToInt32(element.GetProperty("QtyBackOrder").GetString());

                    item.Description = element.GetProperty("Description").GetString();
                    

                
                    string cat = element.GetProperty("BrandID").GetString();
                    // add the FK here
                    foreach (Brand Brand in Brands)
                    {
                        if (Brand.Name == cat)
                        {
                            item.Brand = Brand;
                            break;
                        }
                    }
                    await _db.Products.AddAsync(item);
                    await _db.SaveChangesAsync();
                }


                loadedItems = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedItems;
        }



    }
}
