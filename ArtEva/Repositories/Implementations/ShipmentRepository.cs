using ArteEva.Data;
using ArteEva.Models;

namespace ArteEva.Repositories
{
    public class ShipmentRepository : Repository<Shipment>, IShipmentRepository
    {
        public ShipmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
