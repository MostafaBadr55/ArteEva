using Microsoft.AspNetCore.Identity;

namespace ArteEva.Models
{
    public class UserRole : IdentityUserRole<int>
    {
        // Navigation Properties
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
