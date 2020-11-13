using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.IO;
using Xunit;

namespace SeekQ.Identity.Test
{
    public class BaseIntegrationTest<TStartup>
           : IClassFixture<WebApplicationFactory<TStartup>>
           where TStartup : class
    {
        protected readonly WebApplicationFactory<TStartup> Factory;
        public BaseIntegrationTest(WebApplicationFactory<TStartup> factory)
        {
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            Factory = factory.WithWebHostBuilder(builder => {
                builder.ConfigureAppConfiguration((context, config) => {
                    config.AddJsonFile(configPath);
                });
            });
        }
    }
}