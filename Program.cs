﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RedditBot.Data;
using RedditBot.Data.Repositories;
using RedditBot.Models.Domain;
using RedditBot.Models.Repositories;
using RedditBot.Services;
using System.IO;

namespace RedditBot {
    class Program {
        static void Main(string[] args) {
            IConfiguration config = ConfigureSecrets();
            ServiceProvider services = ConfigureDependecyInjection(config);

            Startup startup = new Startup(services);

            startup.Configure();
            startup.Run().Wait();
        }

        public static IConfiguration ConfigureSecrets() {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Secrets>()
                .Build();
        }

        public static ServiceProvider ConfigureDependecyInjection(IConfiguration configuration) {
            return new ServiceCollection()
                .AddDbContext<ApplicationDBContext>(options => options.UseSqlite(configuration.GetConnectionString("dbcontext")))
                .AddSingleton(new Secrets(configuration.GetSection(nameof(Secrets))))
                .AddSingleton<RedditService>()
                .AddSingleton<IChannelRepository, ChannelRepository>()
                .AddSingleton<IMessageRepository,MessageRepository>()
                .AddSingleton<IGuildRepository,GuildRepository>()
                .AddSingleton<IPrefixRepository,PrefixRepository>()
                .AddOptions()
                .BuildServiceProvider();
        }
    }
}
