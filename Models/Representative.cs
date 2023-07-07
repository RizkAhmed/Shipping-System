using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping_System.Models
{
    public class Representative
    {
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; }


        [Display(Name = "Company Percentage Of Order")]
        public decimal CompanyPercentageOfOrder { get; set; }

        [Display(Name = "Governorate")]
        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public virtual Governorate? Governorate { get; set; }

        [Display(Name = "Branch")]
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public virtual Branch? Branch { get; set; }

        [Display(Name = "Discount Type")]
        [ForeignKey("DiscountType")]
        public int DiscountTypeId { get; set; } 
        public DiscountType? DiscountType { get; set; }
        public bool IsDeleted { get; set; }

    }
}
