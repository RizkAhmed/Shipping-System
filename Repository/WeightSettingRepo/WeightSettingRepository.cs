using Shipping_System.Data;
using Shipping_System.Models;
using System;

namespace Shipping_System.Repository.WeightSettingRepo
{
    public class WeightSettingRepository : IWeightSettingRepository
    {
        ApplicationDbContext _context;

        public WeightSettingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<WeightSetting> GetAll()
        {
            return _context.WeightSetting.ToList();
        }

        public WeightSetting GetById(int id)
        {
            return _context.WeightSetting.Find(id);
        }

        public void Update(WeightSetting weightSetting)
        {
            _context.WeightSetting.Update(weightSetting);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
