using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping_System.Models
{
    public class TraderSpecialPriceForCities
    {
        [Key]
        public int Id { get; set; }

        public int Shippingprice { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey("Trader")]
        public string AppUserId { get; set; }
        public Trader? Trader { get; set; }
        

        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City? City { get; set; }
    }
}
