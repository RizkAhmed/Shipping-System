using Shipping_System.Data;
using Shipping_System.Models;
using System;

namespace Shipping_System.Repository.GovernorateRepo
{
    public class GovernRepository : IGovernRepository  
    {
        ApplicationDbContext _context;
        public GovernRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public void Add(Governorate government)
        {
            _context.Governorates.Add(government);
        }

        public void Delete(int id)
        {
            Governorate governorate = GetById(id);
            _context.Governorates.Remove(governorate);

        }

        public void Edit(Governorate governorate)
        {
            _context.Entry(governorate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public List<Governorate> GetAll()
        {
            return _context.Governorates.ToList();
        }

        public Governorate GetById(int id)
        {
            return _context.Governorates.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
