﻿using Newtonsoft.Json;

namespace IndiegalaFreebieNotifier.Model.PostContent {
	public class QQHttpPostContent {
		[JsonProperty("user_id")]
		public string UserID { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }
	}
}
