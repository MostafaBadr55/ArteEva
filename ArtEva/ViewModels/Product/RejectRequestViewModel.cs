using System.ComponentModel.DataAnnotations;

namespace ArtEva.ViewModels.Product
{
    public class RejectRequestViewModel
    {
        public int ProductId { get; set; }
        [Required]
        public string RejectionMessage { get; set; }
    }
}
