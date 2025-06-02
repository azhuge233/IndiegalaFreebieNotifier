using System.Text;

namespace IndiegalaFreebieNotifier.Model {
	public class FreeGameRecord {
		public string Url { get; set; }

		public string Title { get; set; }

		public string ID { get; set; }

		public string ToTelegramMessage() {
			return string.Format(NotifyFormatStrings.telegramPushFormat, Title, Url);
		}

		public string ToBarkMessage() {
			return string.Format(NotifyFormatStrings.barkPushFormat, Title, Url);
		}

		public string ToEmailMessage() {
			return string.Format(NotifyFormatStrings.emailPushHtmlFormat, Title, Url);
		}

		public string ToQQMessage() {
			return string.Format(NotifyFormatStrings.qqPushFormat, Title, Url);
		}

		public string ToPushPlusMessage() {
			return string.Format(NotifyFormatStrings.pushPlusPushHtmlFormat, Title, Url);
		}

		public string ToDingTalkMessage() {
			return string.Format(NotifyFormatStrings.dingTalkPushFormat, Title, Url);
		}

		public string ToPushDeerMessage() {
			return string.Format(NotifyFormatStrings.pushDeerPushFormat, Title, Url);
		}

		public string ToDiscordMessage() {
			return string.Format(NotifyFormatStrings.discordPushFormat, Url);
		}

		public string ToMeowMessage() {
			return string.Format(NotifyFormatStrings.meowPushFormat, Title, Url);
		}
	}
}