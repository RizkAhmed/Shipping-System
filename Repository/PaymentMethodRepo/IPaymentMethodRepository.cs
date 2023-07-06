using Shipping_System.Models;

namespace Shipping_System.Repository.PaymentMethodRepo
{
    public interface IPaymentMethodRepository
    {
        List<PaymentMethod> GetAll();
    }
}
