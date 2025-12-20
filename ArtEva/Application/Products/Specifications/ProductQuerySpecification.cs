using ArteEva.Models;
using ArtEva.Application.Products.Quiries;
using System.Linq.Expressions;

namespace ArtEva.Application.Products.Specifications
{
    public class ProductQuerySpecification: ISpecification<Product>
    {
        public Expression<Func<Product, bool>> Criteria { get; }

        public ProductQuerySpecification(ProductQueryCriteria criteria)
        {
            Criteria = p =>
                (!criteria.ShopId.HasValue || p.ShopId == criteria.ShopId.Value) &&
                (!criteria.Status.HasValue || p.Status == criteria.Status.Value) &&
                (!criteria.ApprovalStatus.HasValue || p.ApprovalStatus == criteria.ApprovalStatus.Value) &&
                (!criteria.IsPublished.HasValue || p.IsPublished == criteria.IsPublished.Value);
        }
    }
}
