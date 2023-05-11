namespace SimiiformesWebApplication.Models
{
    public class EventPersonConnection
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int PersonId { get; set; }

        public Event Event { get; set; } = null!;

        public Person Person { get; set; } = null!;
    }
}