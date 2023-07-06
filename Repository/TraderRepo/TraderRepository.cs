using Microsoft.EntityFrameworkCore;
using Shipping_System.Data;
using Shipping_System.Models;
using System;

namespace Shipping_System.Repository.TraderRepo
{
    public class TraderRepository : ITraderRepository
    {
        ApplicationDbContext _context;
        public TraderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Trader> GetAll()
        {
            return _context.Traders
                .Include(t => t.AppUser)
                .Include(t => t.Governorate)
                .Include(t => t.City)
                .Include(t => t.Branch).ToList();
        }

        public Trader GetById(string id)
        {
                return _context.Traders
                    .Include(t => t.AppUser)
                    .Include(t => t.Branch)
                    .Include(t => t.Governorate)
                    .Include(t => t.City)
                    .FirstOrDefault(t => t.AppUserId == id)!;
        }

        public void Create(Trader trader)
        {
            _context.Traders.Add(trader);
        }

        public void Edit(Trader trader)
        {
            _context.Traders.Entry(trader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Delete(string id)
        {
            Trader trader = _context.Traders.FirstOrDefault(t => t.AppUserId == id);
            _context.Traders.Remove(trader);
            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
