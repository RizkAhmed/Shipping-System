using Shipping_System.Models;

namespace Shipping_System.Repository.OrderTypeRepo
{
    public interface IOrderTypeRepository
    {
        List<OrderType> GetAll();
    }
}
