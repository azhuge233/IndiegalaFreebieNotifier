using HtmlAgilityPack;
using IndiegalaFreebieNotifier.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IndiegalaFreebieNotifier.Notifier {
	class PushPlus(ILogger<PushPlus> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<PushPlus> _logger = logger;
		private readonly Config config = config.Value;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to PushPlus";
		private readonly string debugCreateMessage = "Create notification message";
		#endregion

		public async Task SendMessage(List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);

				var title = HttpUtility.UrlEncode(new StringBuilder().AppendFormat(NotifyFormatStrings.pushPlusTitleFormat, records.Count).ToString());
				var url = new StringBuilder().AppendFormat(NotifyFormatStrings.pushPlusUrlFormat, config.PushPlusToken, title);
				var message = CreateMessage(records);

				var resp = await new HtmlWeb().LoadFromWebAsync(
					new StringBuilder()
						.Append(url)
						.Append(message)
						.ToString()
				);
				_logger.LogDebug(resp.Text);

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessage}");
				throw;
			} finally {
				Dispose();
			}
		}

		private string CreateMessage(List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(debugCreateMessage);

				var sb = new StringBuilder();

				records.ForEach(record => sb.AppendFormat(NotifyFormatStrings.pushPlusBodyFormat, record.ToPushPlusMessage()));

				sb.Append(NotifyFormatStrings.projectLinkHTML);

				_logger.LogDebug($"Done: {debugCreateMessage}");
				return HttpUtility.UrlEncode(sb.ToString());
			} catch (Exception) {
				_logger.LogError($"Error: {debugCreateMessage}");
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
