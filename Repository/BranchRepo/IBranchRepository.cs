using Shipping_System.Models;

namespace Shipping_System.Repository.BranchRepo
{
    public interface IBranchRepository
    {
        List<Branch> GetAll();
        Branch GetById(int id);
        void Add(Branch branch);
        void Edit(Branch branch);
        void Delete(int id);
        void Save();
    }
}