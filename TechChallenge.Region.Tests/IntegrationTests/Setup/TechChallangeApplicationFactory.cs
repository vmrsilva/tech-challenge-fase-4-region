using DotNet.Testcontainers.Builders;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using TechChallenge.Region.Domain.Cache;
using TechChallenge.Region.Domain.Region.Entity;
using TechChallenge.Region.Infrastructure.Cache;
using TechChallenge.Region.Infrastructure.Context;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace TechChallenge.Region.Tests.IntegrationTests.Setup
{
    public class TechChallangeApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer;
        private readonly RedisContainer _redisContainer;
        private readonly RabbitMqContainer _rabbitMqContainer;
        private readonly string _rabbitPwd = "guest";
        private readonly string _rabbitUser = "guest";
        public TechChallangeApplicationFactory()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _msSqlContainer = new MsSqlBuilder()
                    .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
                      .WithPassword("password(!)Strong")
                             .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                             .Build();
            }
            else
            {
                _msSqlContainer = new MsSqlBuilder().Build();
            }

            _redisContainer = new RedisBuilder().Build();

            _rabbitMqContainer = new RabbitMqBuilder()
                //.WithImage("rabbitmq:3-management")
                .WithImage("masstransit/rabbitmq:latest")
                .WithUsername(_rabbitPwd)
                .WithPassword(_rabbitUser)
                .WithPortBinding(5672, 5672) // Port padrão do RabbitMQ
                .WithPortBinding(15672, 15672) // Port padrão do RabbitMQ Management
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(15672))
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                ConfigureDbContext(services);
                ConfigureCache(services);
                ConfigureRabbitMq(services);
            });

            //builder.UseEnvironment("Development");
            base.ConfigureWebHost(builder);
        }

        private void ConfigureDbContext(IServiceCollection services)
        {
            var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(TechChallangeContext));
            if (context != null)
            {
                services.Remove(context);
                var options = services.Where(r => r.ServiceType == typeof(DbContextOptions)
                  || r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)).ToArray();
                foreach (var option in options)
                {
                    services.Remove(option);
                }
            }

            services.AddDbContext<TechChallangeContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString());

                var connectionString = _msSqlContainer.GetConnectionString();

                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(); // Habilita retry automático
                });

            });


            using (var serviceProvider = services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetRequiredService<TechChallangeContext>();
                dbContext.Database.Migrate();

                SeedRegion(dbContext);
            }
        }

        private void ConfigureCache(IServiceCollection services)
        {
            var cache = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IDistributedCache));
            if (cache != null)
            {
                services.Remove(cache);
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _redisContainer.GetConnectionString();
            });


            services.AddScoped<ICacheRepository, CacheRepository>();
            services.AddScoped<ICacheWrapper, CacheWrapper>();
        }

        //private void ConfigureRabbitMq(IServiceCollection services)
        //{
        //    var rabbitMq = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IBus));
        //    if (rabbitMq != null)
        //    {
        //        services.Remove(rabbitMq);
        //    }

        //    services.AddMassTransit(x =>
        //    {
        //        x.UsingRabbitMq((context, cfg) =>
        //        {
        //            cfg.Host(_rabbitMqContainer.Hostname, "/", h =>
        //            {
        //                h.Username(_rabbitUser);
        //                h.Password(_rabbitPwd);
        //            });

        //            cfg.ConfigureEndpoints(context);
        //        });
        //    });
        //}
        private void ConfigureRabbitMq(IServiceCollection services)
        {
            // Verifica se já existe um IBus registrado (Evita registro duplo)
            var rabbitMq = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IBus));
            if (rabbitMq != null)
            {
                services.Remove(rabbitMq);
            }

            // Também remove qualquer registro do IBusControl e IBusRegistration
            var descriptorsToRemove = services
                .Where(d => d.ServiceType.FullName.Contains("MassTransit"))
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_rabbitMqContainer.Hostname, "/", h =>
                    {
                        h.Username(_rabbitUser);
                        h.Password(_rabbitPwd);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }


        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();

            await _redisContainer.StartAsync();

            await _rabbitMqContainer.StartAsync();
        }

        public async new Task DisposeAsync()
        {
            await _msSqlContainer.StopAsync();
            await _redisContainer.StopAsync();
            await _rabbitMqContainer.StopAsync();
        }

        private void SeedRegion(TechChallangeContext context)
        {
            var regionOne = new RegionEntity("SP", "11");
            var regionTow = new RegionEntity("SC", "47");

            context.Region.AddRange(regionOne, regionTow);

            context.SaveChanges();
        }
    }
}
