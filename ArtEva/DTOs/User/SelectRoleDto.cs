using System.ComponentModel.DataAnnotations;

namespace ArtEva.DTOs.User
{
    public class SelectRoleDto
    {
        [Required(ErrorMessage = "Role is required")]
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Buyer = 1,
        Seller = 2,
        Admin = 3
    }
}
