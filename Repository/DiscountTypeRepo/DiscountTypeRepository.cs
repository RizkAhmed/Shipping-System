using Shipping_System.Data;
using Shipping_System.Models;

namespace Shipping_System.Repository.DiscountTypeRepo
{
    public class DiscountTypeRepository: IDiscountTypeRepository
    {
        ApplicationDbContext _context;
        public DiscountTypeRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public void Add(DiscountType discount)
        {
            _context.DiscountTypes.Add(discount);
        }

        public void Delete(int id)
        {
            DiscountType discount = GetById(id);
            _context.DiscountTypes.Remove(discount);

        }

        public void Edit(DiscountType discount)
        {
            _context.Entry(discount).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public List<DiscountType> GetAll()
        {
            return _context.DiscountTypes.ToList();
        }

        public DiscountType GetById(int id)
        {
            return _context.DiscountTypes.Find( id );
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
