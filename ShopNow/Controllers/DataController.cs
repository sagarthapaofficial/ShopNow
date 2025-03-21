using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopNow.DAL;
using ShopNow.DAL.DAO;
using System.Text.Json;

namespace ShopNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {

        AppDbContext _ctx;
        IWebHostEnvironment _env;
        public DataController(AppDbContext context, IWebHostEnvironment env) // injected here
        {
            _ctx = context;
            _env = env;
        }


        public async Task<ActionResult<String>> Index()
        {
            DataUtility util = new DataUtility(_ctx);
            string payload = "";
            var json = await getProductItemJsonFromWebAsync();
            try
            {
                payload = (await util.loadProductInfoFromWebToDb(json)) ? "tables loaded" : "problem loading tables";
            }
            catch (Exception ex)
            {
                payload = ex.Message;
            }
            return JsonSerializer.Serialize(payload);
        }


        private async Task<String> getProductItemJsonFromWebAsync()
        {
            string url = "https://raw.githubusercontent.com/sagarthapaofficial/SnotifyFile/refs/heads/main/file.json";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }


        [Route("loadstores")]
        public async Task<ActionResult<String>> LoadStores()
        {
            string payload = "";
            StoreDAO dao = new StoreDAO(_ctx);
            bool storesLoaded = await dao.LoadStoresFromFile(_env.WebRootPath);
            try
            {
                payload = storesLoaded ? "stores loaded successfully" : "problem loading store data";
            }
            catch (Exception ex)
            {
                payload = ex.Message;
            }
            return JsonSerializer.Serialize(payload);
        }
    }
}
