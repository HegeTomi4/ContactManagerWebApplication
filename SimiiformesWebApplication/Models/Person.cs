using System.ComponentModel.DataAnnotations.Schema;

namespace SimiiformesWebApplication.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string? Notes { get; set; }
        public string? ImagePath { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        //Pozíció előzmények táblával összeköttetés
        public ICollection<History>? Histories { get; set; }

        public Person()
        {

        }
    }
}
