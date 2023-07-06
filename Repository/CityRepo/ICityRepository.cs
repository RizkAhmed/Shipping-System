using Shipping_System.Models;

namespace Shipping_System.Repository.CityRepo
{
    public interface ICityRepository
    {
        List<City> GetAll();
        List<City> GetAllCitiesByGovId(int id);
        City GetById(int id);
        void Add(City city);
        void Edit(City city);
        void Delete(int id);
        void Save();

    }
}
