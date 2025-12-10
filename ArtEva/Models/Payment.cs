using ArtEva.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
    public class Payment : BaseModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod Method { get; set; }

        public PaymentStatus Status { get; set; }

        [MaxLength(200)]
        public string ProviderRef { get; set; }

        // Navigation Properties
        public Order Order { get; set; }
        public User User { get; set; }
        public ICollection<Refund> Refunds { get; set; }
    }
}
