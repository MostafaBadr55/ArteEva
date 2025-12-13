using ArtEva.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{

    public class Product : BaseModel
    {
      

        [Required]
        [MaxLength(160)]
        public string Title { get; set; }

        [Required]
        [MaxLength(80)]
        public string SKU { get; set; }


        public decimal Price { get; set; }

        public ProductStatus Status { get; set; } = ProductStatus.InActive;
        public ProductApprovalStatus ApprovalStatus { get; set; } = ProductApprovalStatus.Pending;

        public bool IsPublished { get; set; } = false;
        public string? RejectionMessage { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public int ShopId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        // Navigation Properties
        #region nav
        public Shop Shop { get; set; }
        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Favorite> Favorites { get; set; }

        #endregion
    }
}
