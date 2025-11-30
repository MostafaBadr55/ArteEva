using System;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
    public enum ReviewStatus
    {
        Pending,
        Approved,
        Rejected,
        Flagged
    }

    public class Review : BaseModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int OrderItemId { get; set; }

        public int Rating { get; set; } // 1-5

        [MaxLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        public ReviewStatus Status { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Product Product { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}
