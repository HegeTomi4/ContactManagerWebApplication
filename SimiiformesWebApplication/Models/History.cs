
namespace SimiiformesWebApplication.Models
{
    public class History
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string? Company { get; set; }
        public string? Position { get; set; }

        public Person? Person { get; set; }
    }

}
