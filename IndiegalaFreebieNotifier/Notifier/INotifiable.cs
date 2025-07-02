using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using IndiegalaFreebieNotifier.Model;

namespace IndiegalaFreebieNotifier.Notifier {
	interface INotifiable : IDisposable {
		public Task SendMessage(List<FreeGameRecord> records);
	}
}