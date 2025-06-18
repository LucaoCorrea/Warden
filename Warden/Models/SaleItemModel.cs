namespace Warden.Models
{
    public class SaleItemModel
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public SaleModel? Sale { get; set; }

        public int ProductId { get; set; }
        public ProductModel? Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }


        public decimal Total => Quantity * UnitPrice;
    }
}
