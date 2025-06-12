using Warden.Enums;

namespace Warden.Models
{
    public class StockMovementModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public ProductModel? Product { get; set; } 

        public MovementTypeEnum Type { get; set; }

        public int Quantity { get; set; }

        public decimal TotalValue { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
