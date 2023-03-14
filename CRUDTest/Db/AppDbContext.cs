using CRUDTest.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDTest.Db
{
    public class AppDbContext : DbContext
    {
        public DbSet<DrillBlock> DrillBlocks { get; set; } = null!;
        public DbSet<Hole> Holes { get; set; } = null!;
        public DbSet<DrillBlockPoint> DrillBlockPoints { get; set; } = null!;
        public DbSet<HolePoint> HolePoints { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
