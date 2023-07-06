using Shipping_System.Data;
using Shipping_System.Models;
using System;

namespace Shipping_System.Repository.PaymentMethodRepo
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        ApplicationDbContext _context;

        public PaymentMethodRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<PaymentMethod> GetAll()
        {
            return _context.PaymentMethods.ToList();
        }
    }
}
