using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{

    public class Product : BaseModel
    {
        public int ShopId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }

        [Required]
        [MaxLength(160)]
        public string Title { get; set; }

        [Required]
        [MaxLength(80)]
        public string SKU { get; set; }

        public decimal Price { get; set; }

        public bool IsPublished { get; set; } = false;

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // Navigation Properties
        public Shop Shop { get; set; }
        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}
