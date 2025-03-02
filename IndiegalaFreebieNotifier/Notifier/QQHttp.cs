using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IndiegalaFreebieNotifier.Model;
using System.Net.Http;
using IndiegalaFreebieNotifier.Model.PostContent;
using Newtonsoft.Json;

namespace IndiegalaFreebieNotifier.Notifier {
	class QQHttp: INotifiable {
		private readonly ILogger<QQHttp> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notifications to QQ Http";
		#endregion

		public QQHttp(ILogger<QQHttp> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);

				string url = string.Format(NotifyFormatStrings.qqUrlFormat, config.QQHttpAddress, config.QQHttpPort, config.QQHttpToken);

				var client = new HttpClient();

				var content = new QQHttpPostContent {
					UserID = config.ToQQID
				};

				var data = new StringContent(string.Empty);
				var resp = new HttpResponseMessage();

				foreach (var record in records) {
					_logger.LogDebug($"{debugSendMessage} : {record.Title}");

					content.Message = $"{string.Format(NotifyFormatStrings.qqMessageFormat, record.ToQQMessage())}{NotifyFormatStrings.projectLink}";

					data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
					resp = await client.PostAsync(url, data);

					_logger.LogDebug(await resp.Content.ReadAsStringAsync());
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
