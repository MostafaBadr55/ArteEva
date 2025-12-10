using ArtEva.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
   
    public class Review : BaseModel
    {
        // The user who wrote the review (buyer or seller)
        public int ReviewerUserId { get; set; }

        // Identify what is being reviewed: Product, Shop, or Buyer
        public ReviewTargetType TargetType { get; set; }

        // This will refer to: ProductId, ShopId, or BuyerId depending on TargetType
        public int TargetId { get; set; }

        // Rating (e.g., 1–5)
        public int Rating { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        public ReviewStatus Status { get; set; }

        // Navigation: the writer of the review
        public User Reviewer { get; set; }

        // Optional navigation properties (only one will be used depending on TargetType)
        public Product Product { get; set; }
        public Shop Shop { get; set; }

        // If the review is about a buyer
        public User ReviewedBuyer { get; set; }
    }

}
