using System.Text.Json;
using System.Text;

namespace PromoWeb.Web
{
	public class QuestionService : IQuestionService
	{
		private readonly HttpClient _httpClient;

		public QuestionService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<IEnumerable<QuestionListItem>> GetQuestions(int offset = 0, int limit = 10)
		{
			string url = $"{Settings.ApiRoot}/v1/questions?offset={offset}&limit={limit}";

			var response = await _httpClient.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}

			var data = JsonSerializer.Deserialize<IEnumerable<QuestionListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<QuestionListItem>();

			return data;
		}

		public async Task<QuestionListItem> GetQuestion(int questionId)
		{
			string url = $"{Settings.ApiRoot}/v1/questions/{questionId}";

			var response = await _httpClient.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}

			var data = JsonSerializer.Deserialize<QuestionListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new QuestionListItem();

			return data;
		}

		public async Task AddQuestion(QuestionModel model)
		{
			string url = $"{Settings.ApiRoot}/v1/questions";

			var body = JsonSerializer.Serialize(model);
			var request = new StringContent(body, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(url, request);

			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}

		public async Task DeleteQuestion(int questionId)
		{
			string url = $"{Settings.ApiRoot}/v1/questions/{questionId}";

			var response = await _httpClient.DeleteAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}
	}
}
