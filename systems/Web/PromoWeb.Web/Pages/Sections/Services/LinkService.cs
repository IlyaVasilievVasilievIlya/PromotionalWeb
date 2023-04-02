using System.Text.Json;
using System.Text;

namespace PromoWeb.Web
{
	public class LinkService : ILinkService
	{
		private readonly HttpClient _httpClient;

		public LinkService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<IEnumerable<LinkListItem>> GetLinks(int offset = 0, int limit = 10)
		{
			string url = $"{Settings.ApiRoot}/v1/links?offset={offset}&limit={limit}";

			var response = await _httpClient.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}

			var data = JsonSerializer.Deserialize<IEnumerable<LinkListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<LinkListItem>();

			return data;
		}

		public async Task<IEnumerable<LinkListItem>> GetLinksBySectionId(int sectionId)
		{
			string url = $"{Settings.ApiRoot}/v1/links/bySection/{sectionId}";

			var response = await _httpClient.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}

			var data = JsonSerializer.Deserialize<IEnumerable<LinkListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<LinkListItem>();

			return data;
		}

		public async Task<LinkListItem> GetLink(int linkId)
		{
			string url = $"{Settings.ApiRoot}/v1/links/{linkId}";

			var response = await _httpClient.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}

			var data = JsonSerializer.Deserialize<LinkListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new LinkListItem();

			return data;
		}


		public async Task AddLink(LinkModel model)
		{
			string url = $"{Settings.ApiRoot}/v1/links";

			var body = JsonSerializer.Serialize(model);
			var request = new StringContent(body, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(url, request);

			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}

		public async Task EditLink(int linkId, LinkModel model)
		{
			string url = $"{Settings.ApiRoot}/v1/links/{linkId}";

			var body = JsonSerializer.Serialize(model);
			var request = new StringContent(body, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(url, request);

			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}

		public async Task DeleteLink(int linkId)
		{
			string url = $"{Settings.ApiRoot}/v1/links/{linkId}";

			var response = await _httpClient.DeleteAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}

	}
}
