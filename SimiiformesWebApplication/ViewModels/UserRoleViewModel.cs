using System.ComponentModel;

namespace SimiiformesWebApplication.ViewModels
{
    public class UserRoleViewModel
    {
        public string Id { get; set; } = null!;

        [DisplayName("Felhasználó név")]
        public string? UserName { get; set; }        

        public bool AdministratorIsChecked { get; set; }

        public bool ManagerIsChecked { get; set; }

        public bool SystemAdminIsChecked { get; set; }
    }
}