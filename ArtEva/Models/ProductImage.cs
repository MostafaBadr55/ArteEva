using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
    public class ProductImage : BaseModel
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(400)]
        public string Url { get; set; }

        [MaxLength(200)]
        public string AltText { get; set; }

        public int SortOrder { get; set; }

        public bool IsPrimary { get; set; }

        // Navigation Properties
        public Product Product { get; set; }

    }
}
