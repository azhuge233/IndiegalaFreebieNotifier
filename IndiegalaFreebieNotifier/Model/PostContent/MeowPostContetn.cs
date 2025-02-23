using Newtonsoft.Json;

namespace SteamDBFreeGames.Models.PostContent {
	public class MeowPostContent {
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("msg")]
		public string Message { get; set; }
		[JsonProperty("url")]
		public string Url { get; set; }
	}
}