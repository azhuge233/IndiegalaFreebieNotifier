using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;
using IndiegalaFreebieNotifier.Model;

namespace IndiegalaFreebieNotifier.Notifier {
	class TgBot : INotifiable {
		private readonly ILogger<TgBot> _logger;

		public TgBot(ILogger<TgBot> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<FreeGameRecord> records) {
			if (records.Count == 0) {
				_logger.LogInformation("No new notifications !");
				return;
			}

			var BotClient = new TelegramBotClient(config.TelegramToken);

			try {
				foreach (var record in records) {
					_logger.LogDebug("Sending Message {0}", record.Title);
					await BotClient.SendTextMessageAsync(
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
