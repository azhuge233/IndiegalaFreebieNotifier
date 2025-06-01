using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using IndiegalaFreebieNotifier.Notifier;

namespace IndiegalaFreebieNotifier.Module {
    public static class DI {
        private static readonly IConfigurationRoot logConfig = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .Build();
        public static IServiceProvider BuildDiAll() {
            return new ServiceCollection()
               .AddTransient<JsonOP>()
               .AddTransient<ConfigValidator>()
               .AddTransient<Scraper>()
               .AddTransient<Parser>()
               .AddTransient<NotifyOP>()
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
               .AddLogging(loggingBuilder => {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(logConfig);
               })
               .BuildServiceProvider();
        }

        public static IServiceProvider BuildDiNotifierOnly() {
            return new ServiceCollection()
               .AddTransient<TgBot>()
               .AddTransient<Barker>()
               .AddTransient<Email>()
               .AddTransient<QQHttp>()
			   .AddTransient<QQWebSocket>()
			   .AddTransient<PushPlus>()
               .AddTransient<DingTalk>()
               .AddTransient<PushDeer>()
			   .AddTransient<Discord>()
			   .AddTransient<Meow>()
			   .AddLogging(loggingBuilder => {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(logConfig);
               })
               .BuildServiceProvider();
        }

		public static IServiceProvider BuildDiScraperOnly() {
			return new ServiceCollection()
			   .AddTransient<Scraper>()
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