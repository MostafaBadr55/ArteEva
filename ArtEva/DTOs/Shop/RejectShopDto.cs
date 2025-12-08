using System.ComponentModel.DataAnnotations;

namespace ArtEva.DTOs.Shop
{
    public class RejectShopDto
    {
        [Required(ErrorMessage = "Rejection message is required")]
        [MaxLength(1000, ErrorMessage = "Rejection message cannot exceed 1000 characters")]
        public string RejectionMessage { get; set; }
    }
}
