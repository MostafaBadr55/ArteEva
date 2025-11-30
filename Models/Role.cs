using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
    public class Role : BaseModel
    {
        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        // Navigation Properties
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
