using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuitSmoking.Domain.Entities;

namespace QuitSmoking.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cigarretes> Cigarretes { get; set; }
        public DbSet<SmokingHistory> SmokingHistories { get; set; }
    }
}
