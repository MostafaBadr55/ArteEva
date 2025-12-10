using ArtEva.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
  

    public class Refund : BaseModel
    {
        public int OrderId { get; set; }
        public int PaymentId { get; set; }

        public decimal Amount { get; set; }

        public RefundStatus Status { get; set; }

        [MaxLength(500)]
        public string Reason { get; set; }

        // Navigation Properties
        public Order Order { get; set; }
        public Payment Payment { get; set; }
    }
}
