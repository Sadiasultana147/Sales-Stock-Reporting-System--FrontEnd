using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SSRS.Web.Areas.Products.Models;
using SSRS.Web.Areas.Sales.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SSRS.Web.Areas.Sales.Controllers
{
    [Area("Sales")]
    [Authorize]
    public class SalesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SalesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Sales()
        {
            var model = new SalesViewModel();
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync("/api/Products/GetAllProduct");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<List<ProductViewModel>>(json);

                model.ProductList = products.Select(p => new SelectListItem
                {
                    Text = p.ProductName,
                    Value = p.Id.ToString()
                }).ToList();

                model.ProductList.Insert(0, new SelectListItem { Text = "Select a Product", Value = "0" });
            }
            else
            {
                model.ProductList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Failed fetch products", Value = "0" }
                };
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SalesList()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");

                // API call to fetch products
                var response = await client.GetAsync("/api/Sales");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var sales = JsonSerializer.Deserialize<List<SalesViewModel>>(json);


                    return View("_SalesList", sales);
                }
                else
                {
                    return Json(new { Success = false, Message = "Failed to fetch sales" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] SalesViewModel sales)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.PostAsJsonAsync("/api/Sales", sales);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Json(new { success = true, message = "Sale completed successfully." });
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return Json(new { success = false, message = "Have Not enough stock." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetProductById(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                var response = await client.GetAsync($"/api/Products/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var product = JsonSerializer.Deserialize<ProductViewModel>(json);

                    return Json(new { Success = true, Product = product });
                }
                else
                {
                    return Json(new { Success = false, Message = "Failed to fetch product details" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }


    }
}
