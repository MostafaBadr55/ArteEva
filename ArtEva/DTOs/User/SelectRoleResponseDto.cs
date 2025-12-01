namespace ArtEva.DTOs.User
{
    public class SelectRoleResponseDto
    {
        public string Message { get; set; }
        public string Role { get; set; }
        public string RedirectTo { get; set; }
        public bool RequiresShopCreation { get; set; }
    }
}
