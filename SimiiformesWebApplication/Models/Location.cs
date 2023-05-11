using System.ComponentModel;

namespace SimiiformesWebApplication.Models
{
    public class Location
    {
        public int Id { get; set; }
        [DisplayName("Város")]
        public string City { get; set; } = null!;
        [DisplayName("Utca")]
        public string Street { get; set; } = null!;
        [DisplayName("Házszám")]
        public string HouseNumber { get; set; } = null!;
        [DisplayName("Irányítószám")]
        public string PostalCode { get; set; } = null!;
        
        public override string ToString()
        {
            return $"{PostalCode}, {City}, {Street} {HouseNumber}";
        }
    }
}