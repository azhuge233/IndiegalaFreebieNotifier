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
	internal class PushDeer(ILogger<PushDeer> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<PushDeer> _logger = logger;
		private readonly Config config = config.Value;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to PushDeer";
		#endregion

		public async Task SendMessage(List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);
				var webGet = new HtmlWeb();
				var resp = new HtmlDocument();

				foreach (var record in records) {
					_logger.LogDebug($"{debugSendMessage} : {record.Title}");
					resp = await webGet.LoadFromWebAsync(
						new StringBuilder()
						.AppendFormat(NotifyFormatStrings.pushDeerUrlFormat,
									config.PushDeerToken,
									HttpUtility.UrlEncode(record.ToPushDeerMessage()))
						.Append(HttpUtility.UrlEncode(NotifyFormatStrings.projectLink))
						.ToString()
					);
					_logger.LogDebug(resp.Text);
				}

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessage}");
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
