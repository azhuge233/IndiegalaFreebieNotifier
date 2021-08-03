using System;
using System.IO;
using System.Threading.Tasks;
using NLog;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace IndiegalaFreebieNotifier {
	class Program {
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();
		private static readonly IConfigurationRoot config = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.Build();

		public static IServiceProvider BuildDi() {
			return new ServiceCollection()
				.AddTransient<JsonOP>()
				.AddTransient<TgBot>()
				.AddTransient<Scraper>()
				.AddTransient<Parser>()
				.AddLogging(loggingBuilder => {
					loggingBuilder.ClearProviders();
					loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
					loggingBuilder.AddNLog(config);
				})
				.BuildServiceProvider();
		}

		static async Task Main() {
			try {
				var services = BuildDi();
				logger.Info(" - Start Job -");

				using (services as IDisposable) {
					// Get telegram bot token, chatID and previous records
					var jsonOp = services.GetRequiredService<JsonOP>();
					var tgConfig = jsonOp.LoadConfig(); // token, chatID
					var oldRecords = jsonOp.LoadData(); // old records

					// Get page source
					var scraper = services.GetRequiredService<Scraper>();
					var source = await scraper.GetHtmlSource();
					
					// Parse page source
					var parser = services.GetRequiredService<Parser>();
					var parseResult = parser.Parse(source, oldRecords);
					var pushList = parseResult.Item1; // notification list
					var recordList = parseResult.Item2; // new records list

					// Write new records
					jsonOp.WriteData(recordList);

					//Send notifications
					var tgBot = services.GetRequiredService<TgBot>();
					await tgBot.SendMessage(token: tgConfig["TOKEN"], chatID: tgConfig["CHAT_ID"], pushList, htmlMode: true);
				}

				logger.Info(" - Job End -\n\n");
			} catch (Exception ex) {
				logger.Error(ex.Message);
				logger.Error(ex.InnerException.Message);
			} finally {
				LogManager.Shutdown();
			}
		}
	}
}
