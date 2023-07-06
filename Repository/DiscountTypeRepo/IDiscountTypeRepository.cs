using Shipping_System.Models;

namespace Shipping_System.Repository.DiscountTypeRepo
{
    public interface IDiscountTypeRepository    /* add by salah && Rizk*/
    {
        List<DiscountType> GetAll();
        DiscountType GetById(int id);
        void Add(DiscountType discount);
        void Edit(DiscountType discount);
        void Delete(int id);
        void Save();
    }
}
