using System.Text.Json.Serialization;

namespace SSRS.Web.Areas.Products.Models
{
    public class ProductViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string ProductName { get; set; }

        [JsonPropertyName("sku")]
        public string SKUCode { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("stockQty")]
        public int StockQuantity { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
