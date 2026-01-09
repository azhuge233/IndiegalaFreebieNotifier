using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using IndiegalaFreebieNotifier.Model;
using System.Text.Json;

namespace IndiegalaFreebieNotifier.Module {
	class JsonOP(ILogger<JsonOP> logger) : IDisposable {
		private readonly ILogger<JsonOP> _logger = logger;
		private readonly string configPath = $"{AppDomain.CurrentDomain.BaseDirectory}Config{Path.DirectorySeparatorChar}config.json";
		private readonly string recordPath = $"{AppDomain.CurrentDomain.BaseDirectory}Record{Path.DirectorySeparatorChar}record.json";

		private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

		public void WriteData(List<FreeGameRecord> data) {
			try {
				if (data.Count > 0) {
					_logger.LogDebug("Writing records!");
					string json = JsonSerializer.Serialize(data, _jsonOptions);
					File.WriteAllText(recordPath, string.Empty);
					File.WriteAllText(recordPath, json);
					_logger.LogDebug("Done");
				} else _logger.LogDebug("No records detected, quit writing records");
			} catch (Exception) {
				_logger.LogError("Writing data failed.");
				throw;
			} finally {
				Dispose();
			}
		}

		public List<FreeGameRecord> LoadData() {
			try {
				_logger.LogDebug("Loading previous records");
				var content = JsonSerializer.Deserialize<List<FreeGameRecord>>(File.ReadAllText(recordPath));
				_logger.LogDebug("Done");
				return content;
			} catch (Exception) {
				_logger.LogError("Loading previous records failed.");
				throw;
			}
		}

		public void SaveConfig(Config config) {
			try {
				_logger.LogDebug("Saving config");
				string json = JsonSerializer.Serialize(config, _jsonOptions);
				File.WriteAllText(configPath, string.Empty);
				File.WriteAllText(configPath, json);
				_logger.LogDebug("Done");
			} catch (Exception) {
				_logger.LogError("Saving config failed.");
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
