namespace Shipping_System.ViewModels
{
    public class PermissionsFormViewModel
    {
        public string RoleId{ get; set; }
        public string RoleName { get; set; }
        public List<CheckBoxViewModel> RoleCalims { get; set; }
    }
}
