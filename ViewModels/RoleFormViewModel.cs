using System.ComponentModel.DataAnnotations;

namespace Shipping_System.ViewModels
{
    public class RoleFormViewModel
    {
        [MaxLength(30, ErrorMessage = "Name must be less than 30 char")]
        [MinLength(3, ErrorMessage = "Name must be more than 3 char")]
        public string Name { get; set; }
    }
}
