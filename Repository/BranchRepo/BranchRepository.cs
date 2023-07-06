
using Shipping_System.Data;
using Shipping_System.Models;

namespace Shipping_System.Repository.BranchRepo
{
    public class BranchRepository:IBranchRepository
    {
        ApplicationDbContext _context;
        public BranchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Branch branch)
        {
            _context.Branch.Add(branch);
        }

        public void Delete(int id)
        {
            Branch branch = GetById(id);
            branch.IsDeleted = true;
        }

        public void Edit(Branch branch)
        {
            _context.Entry(branch).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public List<Branch> GetAll()
        {
            return _context.Branch.ToList();
        }

        public Branch GetById(int id)
        {
            return _context.Branch.FirstOrDefault(i => i.Id == id)!;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
