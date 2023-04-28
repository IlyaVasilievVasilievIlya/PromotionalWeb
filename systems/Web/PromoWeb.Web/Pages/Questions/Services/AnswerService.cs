using System.Text.Json;
using System.Text;

namespace PromoWeb.Web
{
	public class AnswerService : IAnswerService
	{
		private readonly HttpClient _httpClient;

		public AnswerService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<IEnumerable<AnswerListItem>> GetAnswers(int offset = 0, int limit = 10)
		{
			string url = $"{Settings.ApiRoot}/v1/answers?offset={offset}&limit={limit}";

			var response = await _httpClient.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}

			var data = JsonSerializer.Deserialize<IEnumerable<AnswerListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AnswerListItem>();

			return data;
		}

		public async Task AddAnswer(AnswerModel model)
		{
			string url = $"{Settings.ApiRoot}/v1/answers";

			var body = JsonSerializer.Serialize(model);
			var request = new StringContent(body, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(url, request);

			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}

		public async Task DeleteAnswer(int answerId)
		{
			string url = $"{Settings.ApiRoot}/v1/answers/{answerId}";

			var response = await _httpClient.DeleteAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}
	}
}
