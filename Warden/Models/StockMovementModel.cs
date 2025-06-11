using Warden.Enums;

namespace Warden.Models
{
    public class StockMovementModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
        public MovementTypeEnum Type { get; set; } // Entrada, Saída
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
    }
}
