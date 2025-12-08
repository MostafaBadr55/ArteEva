using System.ComponentModel.DataAnnotations;

namespace ArtEva.DTOs.Admin
{
    public class AssignRoleRequestDto
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string RoleName { get; set; }
    }

    public class AssignRoleResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
