using SimiiformesWebApplication.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimiiformesWebApplication.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [DisplayName("Név")]
        public string Name { get; set; } = null!;

        [DisplayName("Dátum")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DisplayName("Helyszín azonosítója")]
        public int LocationId { get; set; }
        //muliselec-t kiválasztást tárolja
        public ICollection<int>? Guests { get; set; }

        [DisplayName("Helyszín")]
        public Location? Location { get; set; } = null!;
    }
}
