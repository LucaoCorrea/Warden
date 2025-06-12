using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Warden.Models
{
    public class CashRegisterModel
    {
        public int Id { get; set; }

        [Required]
        public string OpenedBy { get; set; }

        public DateTime OpenedAt { get; set; } = DateTime.Now;

        public decimal InitialAmount { get; set; }

        public string? ClosedBy { get; set; }

        public DateTime? ClosedAt { get; set; }

        public decimal? FinalAmount { get; set; }

        public decimal? Difference { get; set; }

        public ICollection<CashMovementModel>? Movements { get; set; }
    }
}
