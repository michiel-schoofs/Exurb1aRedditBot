using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RedditBot.Models.Domain;
using RedditBot.Services;
using System.IO;

namespace RedditBot {
    class Program {
        static void Main(string[] args) {
            IConfiguration config = ConfigureSecrets();
            ServiceProvider services = ConfigureDependecyInjection(config);
            Startup startup = services.GetService<Startup>();

            startup.Configure();
            startup.Run().Wait();
        }

        public static IConfiguration ConfigureSecrets() {
            return new ConfigurationBuilder()
                .AddUserSecrets<Secrets>()
                .Build();
        }

        public static ServiceProvider ConfigureDependecyInjection(IConfiguration configuration) {
            return new ServiceCollection()
                .AddSingleton(new Secrets(configuration.GetSection(nameof(Secrets))))
                .AddSingleton<RedditService>()
                .AddSingleton<Startup>()
                .AddOptions()
                .BuildServiceProvider();
        }
    }
}
