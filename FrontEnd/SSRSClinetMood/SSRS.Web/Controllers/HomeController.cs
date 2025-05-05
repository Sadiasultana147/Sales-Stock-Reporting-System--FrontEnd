using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SSRS.Web.Areas.ReportingModule.Models;
using SSRS.Web.Models;
using SSRS.Web.Models.DashBoarChart;


namespace SSRS.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public async Task<IActionResult> Index()
    {
        
        int currentYear = DateTime.Now.Year;
        var productData = await GetProductStockDataForYear(currentYear);
        var saleData = await GetTotalSalesDataForYear(currentYear);

        var viewModel = new DashboardViewModel
        {
            ProductData = productData,
            SaleData = saleData
        };

       
        return View(viewModel);
    }

    // get product stock of current year
    public async Task<List<ProductStockPerMonth>> GetProductStockDataForYear(int year)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        try
        {
           
            var response = await client.GetStringAsync("/api/Products/GetAllProduct");
            _logger.LogInformation("API Response: " + response);

            var products = JsonSerializer.Deserialize<List<ProductStockPerMonth>>(response) ?? new List<ProductStockPerMonth>();

            var monthlyData = new List<ProductStockPerMonth>();
            for (int month = 1; month <= 12; month++)
            {
                monthlyData.Add(new ProductStockPerMonth { Month = month, StockCount = 0 });
            }

            foreach (var product in products)
            {
              
                if (product.CreatedAt.Year == year)
                {
                    int productMonth = product.CreatedAt.Month;

                    
                    var existingMonthData = monthlyData.FirstOrDefault(m => m.Month == productMonth);
                    if (existingMonthData != null)
                    {
                      
                        existingMonthData.StockCount += product.StockQty;
                    }
                }
            }

            return monthlyData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product data");
            return new List<ProductStockPerMonth>();
        }
    }

    // get total salesof current year
    public async Task<List<ProductStockPerMonth>> GetTotalSalesDataForYear(int year)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        try
        {
            var response = await client.GetStringAsync("/api/Sales");
            _logger.LogInformation("API Response: " + response);

            var sales = JsonSerializer.Deserialize<List<ProductStockPerMonth>>(response) ?? new List<ProductStockPerMonth>();

            var monthlySalesData = new List<ProductStockPerMonth>();
            for (int month = 1; month <= 12; month++)
            {
                monthlySalesData.Add(new ProductStockPerMonth { Month = month, TotalProductSold = 0 });
            }

            foreach (var sale in sales)
            {
                if (sale.SaleDate.Year == year)
                {
                    int saleMonth = sale.SaleDate.Month;
                    var existingMonthData = monthlySalesData.FirstOrDefault(m => m.Month == saleMonth);
                    if (existingMonthData != null)
                    {
                        existingMonthData.TotalProductSold += sale.QuantitySold;
                    }
                }
            }
            return monthlySalesData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching sales data");
            return new List<ProductStockPerMonth>();
        }
    }

}






