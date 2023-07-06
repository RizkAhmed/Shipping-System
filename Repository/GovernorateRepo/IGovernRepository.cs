using Shipping_System.Models;

namespace Shipping_System.Repository.GovernorateRepo
{
    public interface IGovernRepository 
    {
        List<Governorate> GetAll();
        Governorate GetById(int id);
        void Add(Governorate governmate);
        void Edit(Governorate governmate);
        void Delete(int id);
        void Save();
    }
}
