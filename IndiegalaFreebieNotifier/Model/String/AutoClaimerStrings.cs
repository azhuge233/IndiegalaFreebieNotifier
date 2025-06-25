using System.Diagnostics;

namespace IndiegalaFreebieNotifier.Model.String {
	internal class AutoClaimerStrings {

		internal static readonly string UserAgentKey = "User-Agent";
		internal static readonly string UserAgentValue = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.27";
		internal static readonly string CookieKey = "Cookie";

		#region debug strings
		internal static readonly string debugAutoClaimDisabled = "AutoClaim is disabled, skipping.";
		internal static readonly string debugNoCookie = "Cookies are not set, cannot perform Auto Claim.";

		internal static readonly string debugFormattedUrl = "Formatted Url: {0}";

		internal static readonly string debugAutoClaim = "Auto Claim";
		internal static readonly string debugClaiming = "Claiming: {0}";
		internal static readonly string debugClaimed = "Claimed: {0}";

		internal static readonly string warningHttpFailed = "Http response failed. status code: {1}";
		internal static readonly string warningClaimFailed = "Claim failed. Game name: {0} | status: {1} | code: {2}";
		#endregion
	}
}
