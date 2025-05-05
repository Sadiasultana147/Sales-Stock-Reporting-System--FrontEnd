using System.Text.Json.Serialization;

namespace SSRS.Web.Areas.ReportingModule.Models
{
    public class ProductStockViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sku")]
        public string SKU { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("stockQty")]
        public int StockQty { get; set; }
        [JsonPropertyName("quantitySold")]
        public int QuantitySold { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }
        public int OpeningStock { get; set; }
        public int TotalSoldQuantity { get; set; }
        public int ClosingStock { get; set; }
        [JsonPropertyName("saleDate")]
        public DateTime SaleDate { get; set; }
    }
}
