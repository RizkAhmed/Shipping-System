using Shipping_System.Models;

namespace Shipping_System.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime creationDate { get; set; }
        public virtual Branch? Branch { get; set; }
        public bool IsDeleted { get; set; }
        public string  Role { get; set; }

    }
}
