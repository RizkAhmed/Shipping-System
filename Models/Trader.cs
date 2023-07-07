using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Shipping_System.Models
{
    public class Trader
    {
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; }

        [Display(Name = "Store Name")]
        public string StoreName { get; set; }

        [Display(Name = "Special Pickup Cost")]
        public int SpecialPickupCost { get; set; }

        [Display(Name = "Trader Tax For Rejected Orders")]
        public int TraderTaxForRejectedOrders { get; set; }

        [Display(Name = "Governorate")]
        [ForeignKey("Governorate")]
        [Required(ErrorMessage = "Please select governorate")]
        public int GoverId { get; set; }
        public virtual Governorate? Governorate { get; set; }

        [Display(Name = "City")]
        [ForeignKey("City")]
        [Required(ErrorMessage = "Please select city")]
        public int CityId { get; set; }
        public virtual City? City { get; set; }


        [Display(Name = "Branch")]
        [ForeignKey("Branch")]
        [Required(ErrorMessage = "Please select branch")]
        public int BranchId { get; set; }
        public virtual Branch? Branch { get; set; }
        public bool IsDeleted { get; set; }

        public List<TraderSpecialPriceForCities>? SpecialPriceForCities { get; set; } =
                             new List<TraderSpecialPriceForCities>();
    }
}
