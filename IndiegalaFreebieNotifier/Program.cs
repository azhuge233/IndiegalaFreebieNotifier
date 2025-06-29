﻿using System;
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

					// check config validity
					var configValidator = services.GetRequiredService<ConfigValidator>();
					configValidator.CheckValid(config);
					
					if (config.EnableCookieAutoRefresh) {
						// check cookie validity
						bool isCookieValid = await configValidator.CheckCookie(config.Cookie);

						// refresh cookie if not valid
						if (!isCookieValid) {
							string newCookie = await services.GetRequiredService<CookieRefresher>().GetCookie(config);
							config.Cookie = newCookie;

							// save new cookie
							jsonOp.SaveConfig(config);
						}
					}

					// Get page source
					var source = await services.GetRequiredService<Scraper>().GetHomeSource();
					//var source = System.IO.File.ReadAllText("test.html");

					// Parse page source
					var parseResult = services.GetRequiredService<Parser>().Parse(source, jsonOp.LoadData());

					//Send notifications
					await services.GetRequiredService<NotifyOP>().Notify(config, parseResult.PushList);

					// Write new records
					jsonOp.WriteData(parseResult.Records);

					// auto claim
					await services.GetRequiredService<AutoClaimer>().Claim(config, parseResult.PushList);
				}

				logger.Info(" - Job End -\n\n");
			} catch (Exception ex) {
				logger.Error(ex.Message);
			} finally {
				LogManager.Shutdown();
			}
		}
	}
}
