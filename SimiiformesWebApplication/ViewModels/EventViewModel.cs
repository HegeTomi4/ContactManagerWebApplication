using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SimiiformesWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
