using Shipping_System.Models;

namespace Shipping_System.Repository.OrderStateRepo
{
    public interface IOrderStateRepository
    {
        List<OrderState> GetAll();
        List<OrderState> GetStatusForEmployee();
        OrderState GetById(int id);
    }
}
