using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;

namespace SSRS.Web.Areas.Sales.Models
{
    public class SalesViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
        [JsonPropertyName("quantitySold")]
        public int QuantitySold { get; set; }
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }
        [JsonPropertyName("skuCode")]
        public string SKUCode { get; set; }
        [JsonPropertyName("stockQuantity")]
        public int StockQty { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }
        public List<SelectListItem> ProductList { get; set; }
    }
}
