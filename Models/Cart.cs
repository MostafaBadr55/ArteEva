using System;
using System.Collections.Generic;

namespace ArteEva.Models
{
    public class Cart : BaseModel
    {
        public int UserId { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
