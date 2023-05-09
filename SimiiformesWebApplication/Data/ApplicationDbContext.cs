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

        public DbSet<Event>? Events { get; set; } = null!;

        public DbSet<Location>? Locations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            //kapcsolat event és location között, egy eventnek egy locationje van
            //builder.Entity<Event>().HasOne(typeof(Location)).WithOne().HasForeignKey(typeof(Event), "LocationId").OnDelete(DeleteBehavior.Restrict);
        }

    }
}