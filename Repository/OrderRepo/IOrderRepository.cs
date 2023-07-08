using Shipping_System.Models;

namespace Shipping_System.Repository.OrderRepo
{
    public interface IOrderRepository
    {
        List<Order> GetAll();
        Order GetById(int id);
        void Add(Order order);
        void Edit(Order order);
        void Delete(int id);
        void Save();

        List<Order> GetByOrderState(int stateId);

        List<Order> GetByRepresentativeId(string represntativeId);

        decimal CalculateTotalPrice(Order order);

        decimal CalculateCityPrice(int cityId);

        decimal CalculateOrderTypePrice(int deliverTypeId);

        decimal CalculatePriceIfShippingToVillage(Order order);

        decimal CalculatePriceWeight(Order order);

    }
}
