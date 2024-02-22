using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicrosoftAssignment2.Models;

namespace MicrosoftAssignment2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Party> Parties { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 

           
            builder.Entity<Invitation>()
                .Property(inv => inv.Status)
                .HasConversion<string>()
                .HasMaxLength(64);

            //--Seed data for Party entities--\\
            builder.Entity<Party>().HasData(
                new Party
                {
                    Id = 5,
                    Description = "Christmas Party",
                    EventDate = new DateTime(2023, 12, 24),
                    Location = "123 Party Lane"
                },
                new Party
                {
                    Id = 6,
                    Description = "New Year's Eve Celebration",
                    EventDate = new DateTime(2023, 12, 31),
                    Location = "Downtown Club"
                }

            );
        }
    }
}