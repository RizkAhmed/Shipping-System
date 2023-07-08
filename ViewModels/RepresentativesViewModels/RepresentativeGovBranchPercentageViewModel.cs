using Shipping_System.Models;
using System.ComponentModel.DataAnnotations;

namespace Shipping_System.ViewModels
{
    public class RepresentativeGovBranchPercentageViewModel
    {

        public string? AppUserId { get; set; }

        [MaxLength(50,
            ErrorMessage = "Name must be less than 50 characters")]
        [MinLength(8,
            ErrorMessage = "Name must be more than 8 characters")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$",
            ErrorMessage = "Email address is not in valid")]

        public string Email { get; set; }

        [DataType(DataType.Password)]
        [RegularExpression(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$",
                    ErrorMessage = "Password must be 8 : 16 characters long</br> with at least one numeric character")]
        public string Password { get; set; }

        public bool IsDeleted { get; set; } = false;

        [RegularExpression(@"^01[0125][0-9]{8}$",
            ErrorMessage = "The Phone number field is not valid")]
        public string Phone { get; set; }

        [MinLength(5,
            ErrorMessage = "Address must be more than 5 characters")]
        [MaxLength(50,
            ErrorMessage = "Address must be less than 50 characters")]
        public string Address { get; set; }

        [Display(Name = "Company Percentage Of Order")]
        public decimal CompanyPercentageOfOrder { get; set; }

        [Display(Name = "Governorate")]

        [Required(
            ErrorMessage = "Please choose governorate")]
        public int GovernorateId { get; set; }
        public List<Governorate>? Governorates { get; set; } //for drop down list

        [Display(Name = "Branch")]
        [Required(
            ErrorMessage = "Please choose branch")]
        public int BranchId { get; set; }
        public List<Branch>? Branchs { get; set; } //for drop down list

        [Display(Name = "Discount Type")]
        public int DiscountTypeId { get; set; }
        public List<DiscountType>? DiscountTypes { get; set; } //for drop down list
    }
}
