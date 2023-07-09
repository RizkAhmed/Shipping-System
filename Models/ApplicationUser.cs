using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping_System.Models
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(50)]
        [MinLength(3, ErrorMessage = "FullName must be more than 2 char")]
        public string Name { get; set; }

        public string Address { get; set; }
        public DateTime creationDate { get; set; } = DateTime.Now;
        [Required]
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("Branch")]
        [Display(Name = "Branch")]
        [Required(ErrorMessage = "Please select branch")]
        public int BranchId { get; set; }
        public virtual Branch? Branch { get; set; }
    }
}
