using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SimiiformesWebApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SimiiformesWebApplication.Models.Person>? Person { get; set; }
        public DbSet<SimiiformesWebApplication.Models.History>? Histories { get; set; }



    }
}