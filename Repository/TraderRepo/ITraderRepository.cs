using Shipping_System.Models;

namespace Shipping_System.Repository.TraderRepo
{
    public interface ITraderRepository
    {
        List<Trader> GetAll();
        Trader GetById(string id);
        void Create(Trader trader);
        void Edit(Trader trader);
        void Delete(string id);
        void Save();
    }
}
