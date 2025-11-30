using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
    public enum ShopStatus
    {
        Active,
        Inactive,
        Pending,
        Suspended
    }

    public class Shop : BaseModel
    {
        public int OwnerUserId { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; }

        [Required]
        [MaxLength(140)]
        public string Slug { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public ShopStatus Status { get; set; }

        public decimal RatingAverage { get; set; }

        public bool IsDeleted { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation Properties
        public User Owner { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ShopFollower> ShopFollowers { get; set; }
    }
}
