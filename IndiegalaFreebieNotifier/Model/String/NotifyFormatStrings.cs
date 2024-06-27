namespace IndiegalaFreebieNotifier.Model {
	public static class NotifyFormatStrings {
		#region ToMessage() format strings
		public static readonly string telegramPushFormat = "<b>Indiegala</b>\n\n" +
			"<i>{0}</i>\n" +
			"Link: {1}\n\n" +
			"#Indiegala";
		public static readonly string barkPushFormat = "{0}\n" +
			"Link: {1}";
		public static readonly string emailPushHtmlFormat = "<b>{0}</b><br><br>" +
			"Link: <a href=\"{1}\">{1}</a></b>";
		public static readonly string qqPushFormat = "{0}\n" +
			"Link: {1}";
		public static readonly string pushPlusPushHtmlFormat = "<b>{0}</b><br><br>" +
			"Link: <a href=\"{1}\">{1}</a></b>";
		public static readonly string dingTalkPushFormat = "{0}\n" +
			"Link: {1}";
		public static readonly string pushDeerPushFormat = "{0}\n" +
			"Link: {1}";
		public static readonly string discordPushFormat = "Link: {0}";
		#endregion

		#region url, title format strings
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
		public static readonly string qqRedUrlFormat = "ws://{0}:{1}";
		public static readonly string qqRedWSConnectPacketType = "meta::connect";
		public static readonly string qqRedWSSendPacketType = "message::send";
		public static readonly string qqMessageFormat = "Indiegala\n\n{0}";

		public static readonly string pushPlusTitleFormat = "{0} new free game(s) - IndiegalaFreebieNotifier";
		public static readonly string pushPlusBodyFormat = "<br>{0}";
		public static readonly string pushPlusUrlFormat = "http://www.pushplus.plus/send?token={0}&template=html&title={1}&content=";

		public static readonly string dingTalkUrlFormat = "https://oapi.dingtalk.com/robot/send?access_token={0}";

		public static readonly string pushDeerUrlFormat = "https://api2.pushdeer.com/message/push?pushkey={0}&&text={1}";
		#endregion

		public static readonly string projectLink = "\n\nFrom https://github.com/azhuge233/IndiegalaFreebieNotifier";
		public static readonly string projectLinkHTML = "<br><br>From <a href=\"https://github.com/azhuge233/IndiegalaFreebieNotifier\">IndiegalaFreebieNotifier</a>";
	}
}