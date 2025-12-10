using ArtEva.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
    

    public class Notification : BaseModel
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        public NotificationType Type { get; set; }

        // Navigation Properties
        public ICollection<UserNotification> UserNotifications { get; set; }
    }
}
