using ArteEva.Models;
using ArtEva.Application.Products.Specifications;
using ArtEva.Application.Shops.Quiries;
using System.Linq.Expressions;

namespace ArtEva.Application.Shops.Specifications
{
    public class ShopQuerySpecification : ISpecification<Shop>
    {
        public Expression<Func<Shop, bool>> Criteria { get; }

        public ShopQuerySpecification(ShopQueryCriteria criteria)
        {
            Criteria = s =>
                (!criteria.OwnerUserId.HasValue || s.OwnerUserId == criteria.OwnerUserId.Value) &&
                (!criteria.Status.HasValue || s.Status == criteria.Status.Value) &&
                (!criteria.MinRating.HasValue || s.RatingAverage >= criteria.MinRating.Value) &&
                (!criteria.MaxRating.HasValue || s.RatingAverage <= criteria.MaxRating.Value) &&
                (string.IsNullOrEmpty(criteria.Name) || s.Name.Contains(criteria.Name));
        }
    }
}
