using Shipping_System.Models;

namespace Shipping_System.Repository.WeightSettingRepo
{
    public interface IWeightSettingRepository
    {
        IEnumerable<WeightSetting> GetAll();
        WeightSetting GetById(int id);
        void Update(WeightSetting weightSetting);
        void Save();
    }
}
