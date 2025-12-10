using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ArteEva.Models
{
    public class User : IdentityUser<int>
    {
        public bool IsSeller { get; set; }

        public bool IsActive { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #region nav Prop
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
        public ICollection<Review> WrittenReviews { get; set; }  // reviews written by the user
        public ICollection<Review> ReceivedReviews { get; set; } // reviews about a buyer

        public Cart Cart { get; set; }

        #endregion
    }
}
