using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
    public class OrderItem : BaseModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPriceSnapshot { get; set; }

        [MaxLength(200)]
        public string ProductTitleSnapshot { get; set; }

        public decimal Subtotal { get; set; }

        // Navigation Properties
        public Order Order { get; set; }
        public Product Product { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
