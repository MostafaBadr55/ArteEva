using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
    public class User : BaseModel
    {
        [Required]
        [MaxLength(120)]
        public string Email { get; set; }

        [Required]
        [MaxLength(80)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(200)]
        public string PasswordHash { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        public bool IsSeller { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool IsActive { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation Properties
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Shop> Shops { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<UserNotification> UserNotifications { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Dispute> Disputes { get; set; }
        public ICollection<ShopFollower> ShopFollowers { get; set; }
        public Cart Cart { get; set; }
    }
}
