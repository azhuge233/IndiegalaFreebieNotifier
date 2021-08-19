﻿namespace IndiegalaFreebieNotifier.Model {
	public static class NotifyFormatStrings {
		public static readonly string telegramPushFormat = "<b>Indiegala 信息</b>\n\n" +
			"<i>{0}</i>\n" +
			"领取链接: {1}";
		public static readonly string barkPushFormat = "{0}\n" +
			"领取链接: {1}";
		public static readonly string emailPushHtmlFormat = "<b>{0}</b><br><br>" +
			"领取链接: {1}</b>";
		public static readonly string qqPushFormat = "{0}\n" +
			"领取链接: {1}";

		public static readonly string barkUrlFormat = "{0}/{1}/";
		public static readonly string barkUrlTitle = "IndiegalaFreebieNotifier/";
		public static readonly string barkUrlArgs =
			"?group=indiegalafreebienotifier" +
			"&copy={0}" +
			"&url={1}" +
			"&isArchive=1" +
			"&sound=calypso";

		public static readonly string emailTitleFormat = "{0} new free game(s) - IndiegalaFreebieNotifier";
		public static readonly string emailBodyFormat = "<br>{0}";

		public static readonly string qqUrlFormat = "http://{0}:{1}/send_private_msg?user_id={2}&message=";
		public static readonly string qqMessageFormat = "Indigala 信息\n\n{0}";
	}
}