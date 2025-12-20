using System.Linq.Expressions;

namespace ArtEva.Application.Products.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }

}
