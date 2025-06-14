using System.ComponentModel.DataAnnotations;
using Warden.Enums;

namespace Warden.Models
{
    public class SaleModel
    {
        public int Id { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Selecione a forma de pagamento")]
        public PaymentMethodEnum PaymentMethod { get; set; }

        [Required(ErrorMessage = "Informe o nome do vendedor")]
        public string UserName { get; set; }

        //public int? LoyalCustomerId { get; set; }

        //public LoyalCustomerModel LoyalCustomer { get; set; }

        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Adicione pelo menos um item na venda")]
        public List<SaleItemModel> Items { get; set; } = new();

    }
}
