using ArtEva.DTOs.Product;
using Microsoft.AspNetCore.Http;
namespace ArtEva.Extensions
{
   
    public static class HttpRequestExtensions
    {
        public static string BuildPublicUrl(this HttpRequest request, string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return null;

            var baseUrl = $"{request.Scheme}://{request.Host}";
            return $"{baseUrl}/{relativePath.TrimStart('/')}";
        }
     
        public static void BuildProductImagesUrls(this HttpRequest request,IEnumerable<ProductWithImagesDto>? products)
        {
            if (products == null) return;
            foreach (var product in products)
            {
                if (product.Images == null) continue;

                foreach (var image in product.Images)
                {
                    image.Url = request.BuildPublicUrl(image.Url);
                }
            }
        }
    }
 
}
