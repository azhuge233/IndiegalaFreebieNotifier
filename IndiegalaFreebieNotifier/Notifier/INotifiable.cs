using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using IndiegalaFreebieNotifier.Model;

namespace IndiegalaFreebieNotifier.Notifier {
	interface INotifiable : IDisposable {
		public Task SendMessage(NotifyConfig config, List<FreeGameRecord> records);
	}
}