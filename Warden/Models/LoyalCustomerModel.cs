using System;
using System.ComponentModel.DataAnnotations;

namespace Warden.Models
{
    public class LoyalCustomerModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string CEP { get; set; }

        public int Age { get; set; }

        [Display(Name = "Total Cashback (R$)")]
        public decimal CashbackBalance { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
