using System.ComponentModel.DataAnnotations;

namespace ArtEva.DTOs.Product
{
    public class ProductToReject
    {
        public int ProductId { get; set; }
        [Required]
        public string RejectionMessage { get; set; }

    }
}
