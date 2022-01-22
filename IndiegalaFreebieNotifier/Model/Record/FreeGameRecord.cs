using System.Text;

namespace IndiegalaFreebieNotifier.Model {
	public class FreeGameRecord {
		public string Url { get; set; }

		public string Title { get; set; }

		public string ToTelegramMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.telegramPushFormat, Title, Url).ToString();
		}

		public string ToBarkMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.barkPushFormat, Title, Url).ToString();
		}

		public string ToEmailMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.emailPushHtmlFormat, Title, Url).ToString();
		}

		public string ToQQMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.qqPushFormat, Title, Url).ToString();
		}

		public string ToPushPlusMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.pushPlusPushHtmlFormat, Title, Url).ToString();
		}

		public string ToDingTalkMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.dingTalkPushFormat, Title, Url).ToString();
		}

		public string ToPushDeerMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.pushDeerPushFormat, Title, Url).ToString();
		}
	}
}