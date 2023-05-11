namespace SimiiformesWebApplication.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        
        public override string ToString()
        {
            return $"{PostalCode}, {City}, {Street} {HouseNumber}";
        }
    }
}