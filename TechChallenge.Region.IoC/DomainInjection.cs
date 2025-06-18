using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechChallenge.Region.Domain.Base.Repository;
using TechChallenge.Region.Domain.Cache;
using TechChallenge.Region.Domain.Region.Repository;
using TechChallenge.Region.Domain.Region.Service;
using TechChallenge.Region.Infrastructure.Cache;
using TechChallenge.Region.Infrastructure.Context;
using TechChallenge.Region.Infrastructure.Repository.Base;
using TechChallenge.Region.Infrastructure.Repository.Region;

namespace TechChallenge.Region.IoC
{
    public static class DomainInjection
    {
        public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureContext(services, configuration);
            ConfigureBase(services);
            ConfigureRegion(services);
            ConfigureCache(services, configuration);
        }

        public static void ConfigureContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TechChallangeContext>(options => options.UseSqlServer(configuration.GetConnectionString("Database")));

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetRequiredService<TechChallangeContext>();
               // dbContext.Database.Migrate();
            }
        }

        public static void ConfigureBase(IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        }
        public static void ConfigureRegion(IServiceCollection services)
        {
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IRegionService, RegionService>();
        }

        public static void ConfigureCache(IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options => {
                options.InstanceName = nameof(CacheRepository);
                options.Configuration = configuration.GetConnectionString("Cache");
            });
            services.AddScoped<ICacheRepository, CacheRepository>();
            services.AddScoped<ICacheWrapper, CacheWrapper>();
        }
    }
}
