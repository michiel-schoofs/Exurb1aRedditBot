using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RedditBot.Models.Domain;
using System.IO;

namespace RedditBot {
    class Program {
        static void Main(string[] args) {
            IConfiguration config = ConfigureSecrets();
            ServiceProvider services = ConfigureDependecyInjection(config);

            IOptions<Secrets> secrets = services.GetService<IOptions<Secrets>>();
            Startup startup = new Startup(secrets.Value);

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
                .Configure<Secrets>(configuration.GetSection(nameof(Secrets)))
                .AddOptions()
                .BuildServiceProvider();
        }
    }
}
