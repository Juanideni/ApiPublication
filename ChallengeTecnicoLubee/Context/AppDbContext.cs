using ChallengeTecnicoLubee.Models;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTecnicoLubee.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Publication> Publications { get; set; }

        public DbSet<ImagePublication> ImagePublications { get; set; }
    }
}
