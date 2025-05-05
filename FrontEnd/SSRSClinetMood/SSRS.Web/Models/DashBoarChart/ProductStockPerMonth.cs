using System.Text.Json.Serialization;

namespace SSRS.Web.Models.DashBoarChart
{
    public class ProductStockPerMonth
    {
        // Product Data
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
        public int Month { get; set; }
        public int StockCount { get; set; }
        [JsonPropertyName("stockQty")]
        public int StockQty { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        // Sales Data
        [JsonPropertyName("quantitySold")]
        public int QuantitySold { get; set; }

        [JsonPropertyName("totalSold")]
        public decimal TotalSold { get; set; }

        [JsonPropertyName("saleDate")]
        public DateTime SaleDate { get; set; }
        public int TotalProductSold { get; set; }
    }
}
