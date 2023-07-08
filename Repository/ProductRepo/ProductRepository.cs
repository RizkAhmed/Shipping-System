
using Shipping_System.Data;
using Shipping_System.Models;

namespace Shipping_System.Repository.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public void Delete(int id)
        {
            Product product = GetById(id);
            _context.Products.Remove(product);
            _context.SaveChanges();

        }

        public void Edit(Product product)
        {
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.Find(id);

        }

        public List<Product> GetByOrderNo(string orderNo)
        {
            return _context.Products.Where(p=>p.OrderNO == orderNo).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
