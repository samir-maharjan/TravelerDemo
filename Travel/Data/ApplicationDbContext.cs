using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Travel.Models;

namespace Travel.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<TravelSummary> TravelSummaries { get; set; }
        public DbSet<TravelItinenaryDetail> TravelItinenaryDetail { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}