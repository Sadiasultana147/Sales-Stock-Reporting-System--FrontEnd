using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSRS.Web.Areas.Products.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SSRS.Web.Areas.Products.Controllers
{
    [Authorize]
    [Area("Products")]
    
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Product()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(ProductViewModel model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync("/api/Products/GetAllProduct");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var products = JsonSerializer.Deserialize<List<ProductViewModel>>(json);

                    var duplicate = products.FirstOrDefault(p => p.ProductName.ToLower() == model.ProductName.ToLower() && p.Id != model.Id);
                    if (duplicate != null)
                    {
                        return Json(new { Success = false, Message = $"Product name '{model.ProductName}' already exists." });
                    }
                }
                else
                {
                    return Json(new { Success = false, Message = "Failed to fetch ProductList" });
                }

                var jsonContent = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                if (model.Id == 0)
                {
                    response = await client.PostAsync("/api/Products", content);
                }
                else
                {
                    response = await client.PutAsync($"/api/Products/{model.Id}", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { Success = true, Message = "Create/Update successful" });
                }
                else
                {
                    return Json(new { Success = false, Message = "API call failed" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct(int Id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync($"/api/Products/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var product = JsonSerializer.Deserialize<ProductViewModel>(json);
                    return Json(new { Success = true, record = product });
                }
                else
                {
                    return Json(new { Success = false, Message = "Product not found" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProductList()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");

                var response = await client.GetAsync("/api/Products/GetAllProduct");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var products = JsonSerializer.Deserialize<List<ProductViewModel>>(json);
                    var sortedProducts = products.OrderByDescending(p => p.Id).ToList();
                    return View("_ProductList", sortedProducts);
                }
                else
                {
                    return Json(new { Success = false, Message = "Failed to fetch products" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.DeleteAsync($"/api/Products/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Product deleted successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete product" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



    }
}




