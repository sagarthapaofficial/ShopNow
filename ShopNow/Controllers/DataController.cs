using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopNow.DAL;
using System.Text.Json;

namespace ShopNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {

        AppDbContext _ctx;
        public DataController(AppDbContext context) // injected here
        {
            _ctx = context;
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
            string url = "https://raw.githubusercontent.com/sagarthapaofficial/SnotifyFile/main/file.json?token=GHSAT0AAAAAACGUAL2ZRHIQ7I6JBPTMK4PAZHGUXWA";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
