using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shipping_System.ViewModels
{
    public class OrderReporttWithOrderByStatusDateViewModel
    {
        [NotMapped]
        public int Id { get; set; }

        [Display(Name = "Serial Number")]
        public int SerialNumber { get; set; }

        public int StatusId { get; set; }
        public string Status { get; set; }

        public string Trader { get; set; }

        public string Client { get; set; }


        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


        public string Governorate { get; set; }
        public string City { get; set; }

        [Display(Name = "Order Price")]
        public decimal OrderPrice { get; set; }

        [Display(Name = "Order Price Recieved")]
        public decimal OrderPriceRecieved { get; set; } = 0;

        [Display(Name = "Shipping Price")]
        public decimal ShippingPrice { get; set; }

        [Display(Name = "Shipping Price Recieved ")]
        public decimal ShippingPriceRecived { get; set; } = 0;

        [Display(Name = "Company Rate")]
        public decimal CompanyRate { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;


        public bool IsDeleted { get; set; } = false;
    }
}
