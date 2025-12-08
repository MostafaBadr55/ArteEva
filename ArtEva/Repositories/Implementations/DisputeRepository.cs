using ArteEva.Data;
using ArteEva.Models;

namespace ArteEva.Repositories
{
    public class DisputeRepository : Repository<Dispute>, IDisputeRepository
    {
        public DisputeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
