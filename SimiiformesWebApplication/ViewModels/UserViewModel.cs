using System.ComponentModel;

namespace SimiiformesWebApplication.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; } = null!;

        [DisplayName("Felhasználó név")]
        public string UserName { get; set; } = null!;

        [DisplayName("Jogosultságok")]
        public string Role { get; set; } = null!;        
    }
}