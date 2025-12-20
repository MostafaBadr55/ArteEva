using ArtEva.Models.Enums;

namespace ArtEva.Application.ShopProduct.Quiries
{
    public class ShopProductQueryCriteria
    {
        public ProductStatus? Status { get; set; }
        public ProductApprovalStatus? ApprovalStatus { get; set; }
        public bool? IsPublished { get; set; }
    }
}
