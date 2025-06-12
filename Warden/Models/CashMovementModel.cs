using System;
using System.ComponentModel.DataAnnotations;
using Warden.Enums;

namespace Warden.Models
{
    public class CashMovementModel
    {
        public int Id { get; set; }

        [Required]
        public int CashRegisterId { get; set; }

        public CashRegisterModel? CashRegister { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public decimal Value { get; set; }

        public MovementTypeEnum Type { get; set; }

        public string Description { get; set; }
    }
}
