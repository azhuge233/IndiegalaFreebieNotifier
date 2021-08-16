using System;
using System.Threading.Tasks;
using NLog;
using Microsoft.Extensions.DependencyInjection;
using IndiegalaFreebieNotifier.Module;

namespace IndiegalaFreebieNotifier {
	class Program {
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		static async Task Main() {
			try {
				var services = DI.BuildDiAll();
				logger.Info(" - Start Job -");

				using (services as IDisposable) {
					// Get telegram bot token, chatID and previous records
					var jsonOp = services.GetRequiredService<JsonOP>();
					var config = jsonOp.LoadConfig();
					services.GetRequiredService<ConfigValidator>().CheckValid(config);

					// Get page source
					var source = await services.GetRequiredService<Scraper>().GetHtmlSource(config);
					//var source = System.IO.File.ReadAllText("test.html");
					
					// Parse page source
					var parseResult = services.GetRequiredService<Parser>().Parse(source, jsonOp.LoadData());

					//Send notifications
					await services.GetRequiredService<NotifyOP>().Notify(config, parseResult.PushList);

					// Write new records
					jsonOp.WriteData(parseResult.Records);
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
