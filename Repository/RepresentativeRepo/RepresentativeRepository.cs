using Microsoft.EntityFrameworkCore;
using Shipping_System.Data;
using Shipping_System.Models;
using Shipping_System.Repository.RepresentiveRepo;

namespace Shipping_System.Repository.RepresentativeRepo
{
    public class RepresentativeRepository : IRepresentativeRepository
    {
        private readonly ApplicationDbContext _context;
        public RepresentativeRepository(ApplicationDbContext context)
        {

            _context = context;

        }

        public void Add(Representative rep)
        {
            _context.Representatives.Add(rep);
        }

        public void Edit(Representative rep)
        {
            _context.Representatives.Entry(rep).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        
        public List<Representative> GetAll()
        {
            return _context.Representatives
                .Include(r=>r.AppUser)
                .Include(r=>r.Branch)
                .Include(r=>r.Governorate)
                .ToList();
        }

        public Representative GetById(string id)
        {
            return _context.Representatives.FirstOrDefault(r=>r.AppUserId==id);
        }

        public void Delete(string id)
        {
            Representative rep = GetById(id);
            rep.IsDeleted = true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public List<Representative> GetByBranchId(int BranchId)
        {
            
            return _context.Representatives.Where(r=>r.BranchId == BranchId).ToList();
        }
    
    }
}
