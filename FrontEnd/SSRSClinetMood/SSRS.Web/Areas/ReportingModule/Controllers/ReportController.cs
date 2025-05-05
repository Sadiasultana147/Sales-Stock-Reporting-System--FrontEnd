
using Microsoft.AspNetCore.Mvc;
using SSRS.Web.Areas.Products.Models;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Reporting.NETCore;
using SSRS.Web.Areas.ReportingModule.Models;
using Microsoft.AspNetCore.Authorization;
using SSRS.Web.Areas.Sales.Models;

namespace SSRS.Web.Areas.ReportingModule.Controllers
{
    [Area("ReportingModule")]
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _env;

        public ReportController(IHttpClientFactory httpClientFactory, IWebHostEnvironment env)
        {
            _httpClientFactory = httpClientFactory;
            _env = env;
        }

        public IActionResult StockReport()
        {
            return View();
        }

        public async Task<IActionResult> PdfGenerates()
        {
            string reportPath = Path.Combine(_env.WebRootPath, "Reports", "ProductList.rdlc");
            if (!System.IO.File.Exists(reportPath))
                return NotFound($"RDLC file not found at {reportPath}");

            var localReport = new Microsoft.Reporting.NETCore.LocalReport();
            localReport.ReportPath = reportPath;
            var productData = await FetchProductDataFromApiAsync();

            if (productData == null || productData.Count == 0)
                return NotFound("No product data found.");
            var productDataSource = new ReportDataSource("DataSet1", productData);
            localReport.DataSources.Add(productDataSource);
            string mimeType, encoding, fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes = localReport.Render(
                "PDF", 
                null, 
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
            );

            string reportName = "ProductListReport";
            Response.Headers.Append("Content-Disposition", $"inline; filename={reportName}.pdf");
            return File(renderedBytes, "application/pdf");
        }

        private async Task<List<ProductStockViewModel>> FetchProductDataFromApiAsync()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            string apiUrl = "/api/Products/GetAllProduct"; 

            try
            {
                var response = await client.GetStringAsync(apiUrl);
                var products = JsonSerializer.Deserialize<List<ProductStockViewModel>>(response);

                return products ?? new List<ProductStockViewModel>();
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"Error fetching data from API: {ex.Message}");
                return new List<ProductStockViewModel>();
            }
        }
        public async Task<IActionResult> dateWisePdfGenerates(DateTime fromDate, DateTime toDate)
        {
            string reportPath = Path.Combine(_env.WebRootPath, "Reports", "DateWiseStock.rdlc");

            if (!System.IO.File.Exists(reportPath))
                return NotFound($"RDLC file not found at {reportPath}");

            var localReport = new Microsoft.Reporting.NETCore.LocalReport();
            localReport.ReportPath = reportPath;

            var productData = await dateWiseProductDataFromApiAsync(fromDate, toDate);
            if (productData == null || productData.Count == 0)
                return NotFound("No product data found.");
            var productDataSource = new ReportDataSource("DataSet1", productData);
            localReport.DataSources.Add(productDataSource);

            var parameters = new List<ReportParameter>
            {
                new ReportParameter("FromDate", fromDate.ToString("yyyy-MM-dd")),
                new ReportParameter("ToDate", toDate.ToString("yyyy-MM-dd"))
            };
            localReport.SetParameters(parameters);

            string mimeType, encoding, fileNameExtension;
            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = localReport.Render(
                "PDF",
                null,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
            );

            string reportName = $"DateWiseStockReport_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}";
            Response.Headers.Append("Content-Disposition", $"inline; filename={reportName}.pdf");

            return File(renderedBytes, "application/pdf");
        }

        private async Task<List<ProductStockViewModel>> dateWiseProductDataFromApiAsync(DateTime fromDate, DateTime toDate)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                var productResponse = await client.GetStringAsync("/api/Products/GetAllProduct");
                var products = JsonSerializer.Deserialize<List<ProductStockViewModel>>(productResponse) ?? new();

                var salesResponse = await client.GetStringAsync("/api/Sales");
                var rawSales = JsonSerializer.Deserialize<List<ProductStockViewModel>>(salesResponse) ?? new();
                var sales = rawSales
                        .Where(s => s.ProductId > 0 && s.QuantitySold > 0 &&
                                    s.SaleDate.Date >= fromDate.Date &&
                                    s.SaleDate.Date <= toDate.Date.AddDays(1).AddTicks(-1)) 
                        .ToList();
                foreach (var sale in sales)
                {
                    Console.WriteLine($"ProductId: {sale.ProductId}, QuantitySold: {sale.QuantitySold}, SaleDate: {sale.SaleDate}");
                }
                foreach (var product in products.Where(p => !p.IsDeleted))
                {
                    var totalSoldInRange = sales
                        .Where(s => s.ProductId == product.Id)
                        .Sum(s => s.QuantitySold);

                    var soldBefore = rawSales
                        .Where(s => s.ProductId == product.Id && s.SaleDate < fromDate)
                        .Sum(s => s.QuantitySold);

                    product.OpeningStock = product.StockQty + soldBefore;
                    product.TotalSoldQuantity = totalSoldInRange;
                    product.ClosingStock = product.OpeningStock - totalSoldInRange;
                }

                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<ProductStockViewModel>();
            }
        }


    }
}
