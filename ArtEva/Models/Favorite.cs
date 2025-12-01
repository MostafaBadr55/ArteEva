using System;

namespace ArteEva.Models
{
    public class Favorite : BaseModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
