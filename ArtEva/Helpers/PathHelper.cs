namespace ArtEva.Helpers
{
    public static class PathHelper
    {
        public static string GetFolderByType(string type)
        {
            return type.ToLower() switch
            {
                "shop" => "shops",
                "product" => "products",
                "category" => "categories",
                "user" => "users",

                _ => "general"
            };
        }
    }

}
