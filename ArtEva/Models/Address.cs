using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
    public class Address : BaseModel
    {
        public int? UserId { get; set; }

        [MaxLength(100)]
        public string Label { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        [MaxLength(20)]
        public string PostalCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string Line1 { get; set; }

        [MaxLength(200)]
        public string Line2 { get; set; }

        public bool IsDefault { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Shipment> Shipments { get; set; }
    }
}
