using ArtEva.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
   

    public class Shop : BaseModel
    {
        public int OwnerUserId { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public ShopStatus Status { get; set; } = ShopStatus.Pending;

        [MaxLength(1000)]
        public string? RejectionMessage { get; set; }

        public decimal RatingAverage { get; set; } = 0;

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // Navigation Properties
        public User Owner { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ShopFollower> ShopFollowers { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
