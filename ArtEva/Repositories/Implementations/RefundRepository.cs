using ArteEva.Data;
using ArteEva.Models;

namespace ArteEva.Repositories
{
    public class RefundRepository : Repository<Refund>, IRefundRepository
    {
        public RefundRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
