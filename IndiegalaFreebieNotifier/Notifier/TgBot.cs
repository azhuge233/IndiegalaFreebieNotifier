using IndiegalaFreebieNotifier.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace IndiegalaFreebieNotifier.Notifier {
	class TgBot(ILogger<TgBot> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<TgBot> _logger = logger;
		private readonly Config config = config.Value;

		public async Task SendMessage(List<FreeGameRecord> records) {
			if (records.Count == 0) {
				_logger.LogInformation("No new notifications !");
				return;
			}

			var BotClient = new TelegramBotClient(config.TelegramToken);

			try {
				foreach (var record in records) {
					_logger.LogDebug("Sending Message {0}", record.Title);
					await BotClient.SendMessage(
						chatId: config.TelegramChatID,
						text: $"{record.ToTelegramMessage()}{NotifyFormatStrings.projectLinkHTML.Replace("<br>", "\n")}",
						parseMode: ParseMode.Html
					);
				}
			} catch (Exception) {
				_logger.LogError("Send notification failed.");
				throw;
			} finally {
				Dispose();
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
