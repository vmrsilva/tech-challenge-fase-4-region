using Microsoft.EntityFrameworkCore;
using TechChallenge.Region.Domain.Region.Entity;

namespace TechChallenge.Region.Infrastructure.Context
{
    public class TechChallangeContext : DbContext
    {
        public TechChallangeContext() : base()
        {
            //Database.Migrate();
        }

        public TechChallangeContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<RegionEntity> Region { get; set; }
    }
}
