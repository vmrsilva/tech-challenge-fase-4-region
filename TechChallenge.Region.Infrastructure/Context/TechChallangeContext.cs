using Microsoft.EntityFrameworkCore;
using TechChallange.Region.Domain.Region.Entity;

namespace TechChallange.Region.Infrastructure.Context
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
