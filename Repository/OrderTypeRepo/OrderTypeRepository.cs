using Shipping_System.Data;
using Shipping_System.Models;
using System;

namespace Shipping_System.Repository.OrderTypeRepo
{
    public class OrderTypeRepository : IOrderTypeRepository
    {
        ApplicationDbContext _context;

        public OrderTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<OrderType> GetAll()
        {
            return _context.OrderTypes.ToList();
        }
    }
}
