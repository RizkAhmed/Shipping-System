using Shipping_System.Data;
using Shipping_System.Models;
using System;

namespace Shipping_System.Repository.OrderStateRepo
{
    public class OrderStateRepository : IOrderStateRepository
    {
        ApplicationDbContext _context;

        public OrderStateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<OrderState> GetAll()
        {
            return _context.OrderStates.ToList();
        }

        //Get OrderStatus for Emp( new - watting - Delivered to the representative)
        public List<OrderState> GetStatusForEmployee()
        {
            return _context.OrderStates.Where(Os=>Os.Id<4& Os.Id>1).ToList();
        }
        public OrderState GetById(int id)
        {
            return _context.OrderStates.FirstOrDefault(e => e.Id == id)!;
        }
    }
}
