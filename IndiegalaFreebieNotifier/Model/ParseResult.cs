using System.Collections.Generic;

namespace IndiegalaFreebieNotifier.Model {
	public class ParseResult {
		public List<FreeGameRecord> PushList { get; set; }

		public List<FreeGameRecord> Records { get; set; }

		public ParseResult() {
			PushList = new List<FreeGameRecord>();
			Records = new List<FreeGameRecord>();
		}
	}
}