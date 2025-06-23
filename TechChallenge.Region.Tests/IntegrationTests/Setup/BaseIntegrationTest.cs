using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TechChallange.Region.Domain.Region.Service;
using TechChallange.Region.Infrastructure.Context;

namespace TechChallange.Region.Tests.IntegrationTests.Setup
{
    public class BaseIntegrationTest : IClassFixture<TechChallangeApplicationFactory>, IDisposable
    {
        private readonly IServiceScope _scope;
        protected readonly JsonSerializerOptions _jsonSerializerOptions;
        protected readonly IRegionService _regionService;
        protected readonly TechChallangeContext _dbContext;

        public BaseIntegrationTest(TechChallangeApplicationFactory factory)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _scope = factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<TechChallangeContext>();
            _regionService = _scope.ServiceProvider.GetRequiredService<IRegionService>();
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}
