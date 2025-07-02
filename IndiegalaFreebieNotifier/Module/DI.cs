using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using IndiegalaFreebieNotifier.Notifier;
using IndiegalaFreebieNotifier.Model;

namespace IndiegalaFreebieNotifier.Module {
    public static class DI {
        private static readonly IConfigurationRoot logConfig = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .Build();
		private static readonly IConfigurationRoot configuration = new ConfigurationBuilder()
		   .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("Config/config.json", optional: false, reloadOnChange: true)
		   .Build();

		public static IServiceProvider BuildDiAll() {
            return new ServiceCollection()
               .AddTransient<JsonOP>()
               .AddTransient<ConfigValidator>()
               .AddTransient<Scraper>()
               .AddTransient<Parser>()
               .AddTransient<NotifyOP>()
               .AddTransient<AutoClaimer>()
               .AddTransient<CookieRefresher>()
               .AddTransient<CaptchaSolver>()
               .AddTransient<Barker>()
               .AddTransient<TgBot>()
               .AddTransient<Email>()
               .AddTransient<QQHttp>()
               .AddTransient<QQWebSocket>()
               .AddTransient<PushPlus>()
               .AddTransient<DingTalk>()
               .AddTransient<PushDeer>()
               .AddTransient<Discord>()
               .AddTransient<Meow>()
               .Configure<Config>(configuration)
               .AddLogging(loggingBuilder => {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(logConfig);
               })
               .BuildServiceProvider();
        }
	}
}