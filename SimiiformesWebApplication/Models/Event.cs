using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimiiformesWebApplication.Models
{
    //események
    public class Event
    {
        public int Id { get; set; }

        [DisplayName("Név")]
        public string Name { get; set; } = null!;

        [DisplayName("Dátum")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DisplayName("Helyszín azonosítója")]
        public int LocationId { get; set; }

        public ICollection<EventPersonConnection>? Guests { get; set; }

        [DisplayName("Helyszín")]
        public Location? Location { get; set; }
    }
}