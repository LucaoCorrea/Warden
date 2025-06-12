using Warden.Enums;

namespace Warden.Models
{
    public class SaleModel
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public PaymentMethodEnum PaymentMethod { get; set; } 
        public string UserName { get; set; }
        public List<SaleItemModel> Items { get; set; }
    }
}
