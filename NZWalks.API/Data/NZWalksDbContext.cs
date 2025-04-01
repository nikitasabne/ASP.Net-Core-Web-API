using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
       

        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> option) : base(option)
        {
        }

        public DbSet<Difficulty> difficulties { get; set; }
        public DbSet<Region> regions { get; set; }
        public DbSet<Walk> walks { get; set; }
    }
}
