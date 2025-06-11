namespace Warden.Models
{
    // model do produto em estoque
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Stock { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }

        public string ImageUrl { get; set; }

        public string Unit { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
