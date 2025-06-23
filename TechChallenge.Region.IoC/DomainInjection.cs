using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechChallange.Common.MessagingService;
using TechChallange.Region.Domain.Base.Repository;
using TechChallange.Region.Domain.Cache;
using TechChallange.Region.Domain.Region.Messaging;
using TechChallange.Region.Domain.Region.Repository;
using TechChallange.Region.Domain.Region.Service;
using TechChallange.Region.Infrastructure.Cache;
using TechChallange.Region.Infrastructure.Context;
using TechChallange.Region.Infrastructure.Repository.Base;
using TechChallange.Region.Infrastructure.Repository.Region;

namespace TechChallange.Region.IoC
{
    public static class DomainInjection
    {
        public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureContext(services, configuration);
            ConfigureBase(services);
            ConfigureRegion(services);
            ConfigureCache(services, configuration);
            ConfigureMessagingService(services, configuration);
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
            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = nameof(CacheRepository);
                options.Configuration = configuration.GetConnectionString("Cache");
            });
            services.AddScoped<ICacheRepository, CacheRepository>();
            services.AddScoped<ICacheWrapper, CacheWrapper>();
        }

        public static void ConfigureMessagingService(IServiceCollection services, IConfiguration configuration)
        {
            var servidor = configuration.GetSection("MassTransit")["Server"] ?? string.Empty;
            var usuario = configuration.GetSection("MassTransit")["User"] ?? string.Empty;
            var senha = configuration.GetSection("MassTransit")["Password"] ?? string.Empty;

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(servidor, "/", h =>
                    {
                        h.Username(usuario);
                        h.Password(senha);
                    });

                    // Configurar o nome da exchange para RegionCreateDto
                    cfg.Message<RegionCreateMessageDto>(m =>
                    {
                        m.SetEntityName("region-insert-exchange"); // Define o nome personalizado da exchange
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddScoped<IMessagingService, MessagingService>();
        }
    }
}
