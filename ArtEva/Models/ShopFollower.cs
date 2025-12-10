using System;

namespace ArteEva.Models
{
    public class ShopFollower : BaseModel
    {
        public int ShopId { get; set; }
        public int UserId { get; set; }

        // Navigation Properties
        public Shop Shop { get; set; }
        public User User { get; set; }
    }
}
