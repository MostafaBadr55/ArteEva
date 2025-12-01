using ArteEva.Data;
using ArteEva.Models;

namespace ArteEva.Repositories
{
    public class UserNotificationRepository : Repository<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
