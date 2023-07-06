using Shipping_System.Data;
using Shipping_System.Models;
using System;

namespace Shipping_System.Repository.DeliverTypeRepo
{
    public class DeliverTypeRepository : IDeliverTypeRepository
    {
        ApplicationDbContext _context;

        public DeliverTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<DeliverType> GetAll()
        {
            return _context.DeliverTypes.ToList();
        }
    }
}
