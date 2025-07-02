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
	class Barker(ILogger<Barker> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<Barker> _logger = logger;
		private readonly Config config = config.Value;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Bark";
		#endregion

		public async Task SendMessage(List<FreeGameRecord> records) {
			try {
				var sb = new StringBuilder();
				string url = new StringBuilder().AppendFormat(NotifyFormatStrings.barkUrlFormat, config.BarkAddress, config.BarkToken).ToString();
				var webGet = new HtmlWeb();

				foreach (var record in records) {
					_logger.LogDebug($"{debugSendMessage} : {record.Title}");
					await webGet.LoadFromWebAsync(
						new StringBuilder()
							.Append(url)
							.Append(NotifyFormatStrings.barkUrlTitle)
							.Append(HttpUtility.UrlEncode(record.ToBarkMessage()))
							.Append(HttpUtility.UrlEncode(NotifyFormatStrings.projectLink))
							.Append(new StringBuilder().AppendFormat(NotifyFormatStrings.barkUrlArgs, record.Url, record.Url))
							.ToString()
					);
				}

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogDebug($"Error: {debugSendMessage}");
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