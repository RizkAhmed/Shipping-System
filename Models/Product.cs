using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping_System.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string OrderNO { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Weight { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        public Order? Order { get; set; }

        public bool IsDeleted { get; set; }
    }
}
