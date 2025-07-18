﻿using IndiegalaFreebieNotifier.Model;
using IndiegalaFreebieNotifier.Model.WebSocketContent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Websocket.Client;

namespace IndiegalaFreebieNotifier.Notifier {
	internal class QQWebSocket(ILogger<QQWebSocket> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<QQWebSocket> _logger = logger;
		private readonly Config config = config.Value;

		#region debug strings
		private readonly string debugSendMessage = "Send notifications to QQ WebSocket";
		private readonly string debugWSReconnection = "Reconnection happened, type: {0}";
		private readonly string debugWSMessageRecieved = "Message received: {0}";
		private readonly string debugWSDisconnected = "Disconnected: {0}";
		#endregion

		public async Task SendMessage(List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);

				var packets = GetSendPacket(config, records);

				using var client = GetWSClient(config);

				await client.Start();

				foreach (var packet in packets) {
					await client.SendInstant(JsonConvert.SerializeObject(packet));
					await Task.Delay(600);
				}

				await client.Stop(WebSocketCloseStatus.NormalClosure, string.Empty);

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogDebug($"Error: {debugSendMessage}");
				throw;
			} finally {
				Dispose();
			}
		}

		private WebsocketClient GetWSClient(NotifyConfig config) {
			var url = new Uri(string.Format(NotifyFormatStrings.qqWebSocketUrlFormat, config.QQWebSocketAddress, config.QQWebSocketPort, config.QQWebSocketToken));

			#region new websocket client
			var client = new WebsocketClient(url);
			client.ReconnectionHappened.Subscribe(info => _logger.LogDebug(debugWSReconnection, info.Type));
			client.MessageReceived.Subscribe(msg => _logger.LogDebug(debugWSMessageRecieved, msg));
			client.DisconnectionHappened.Subscribe(msg => _logger.LogDebug(debugWSDisconnected, msg));
			#endregion

			return client;
		}

		private static List<WSPacket> GetSendPacket(NotifyConfig config, List<FreeGameRecord> records) {
			return records.Select(record => new WSPacket {
				Action = NotifyFormatStrings.qqWebSocketSendAction,
				Params = new Param {
					UserID = config.ToQQID,
					Message = $"{string.Format(NotifyFormatStrings.qqMessageFormat, record.ToQQMessage())}{NotifyFormatStrings.projectLink}"
				}
			}).ToList();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
