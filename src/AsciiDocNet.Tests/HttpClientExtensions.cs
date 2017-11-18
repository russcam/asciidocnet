using System.Net.Http;

namespace AsciiDocNet.Tests
{
	public static class HttpClientExtensions
	{
		public static string DownloadString(this HttpClient client, string url)
		{
			using (var response = client.GetAsync(url).Result)
			using (var content = response.Content)
				return content.ReadAsStringAsync().Result;
		}
	}
}