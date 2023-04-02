
using System.Text;
using System.Text.Json;

namespace PromoWeb.Web
{
    public class SectionService : ISectionService
    {
        private readonly HttpClient _httpClient;

        public SectionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<SectionListItem>> GetSections(int offset = 0, int limit = 10)
        {
            string url = $"{Settings.ApiRoot}/v1/sections?offset={offset}&limit={limit}";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
			//попытка десериализации с парсингом contactresponce -> contactmlistitem,иначе пустой список
            var data = JsonSerializer.Deserialize<IEnumerable<SectionListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<SectionListItem>();

                return data;
        }

        public async Task<SectionListItem> GetSection(int sectionId)
        {
            string url = $"{Settings.ApiRoot}/v1/sections/{sectionId}";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            var data = JsonSerializer.Deserialize<SectionListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new SectionListItem();

            return data;
        }

		public async Task AddSection(SectionModel model)
		{
			string url = $"{Settings.ApiRoot}/v1/sections";

			var body = JsonSerializer.Serialize(model); //в тело запроса
			var request = new StringContent(body, Encoding.UTF8, "application/json"); //кодировка тела запроса
			var response = await _httpClient.PostAsync(url, request);

			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}

		public async Task EditSection(int sectionId, SectionModel model)
		{
			string url = $"{Settings.ApiRoot}/v1/sections/{sectionId}";

			var body = JsonSerializer.Serialize(model);
			var request = new StringContent(body, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(url, request);

			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}

		public async Task DeleteSection(int sectionId)
		{
			string url = $"{Settings.ApiRoot}/v1/sections/{sectionId}";

			var response = await _httpClient.DeleteAsync(url);
			var content = await response.Content.ReadAsStringAsync(); //внутри вернет iactionresult (код ок)

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}

		public async Task<IEnumerable<LinkModel>> GetLinkList()
		{
			string url = $"{Settings.ApiRoot}/v1/links";

			var response = await _httpClient.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}

			var data = JsonSerializer.Deserialize<IEnumerable<LinkModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<LinkModel>();

			return data;
		}

		public async Task<IEnumerable<AppInfoModel>> GetAppInfoList()
		{
			string url = $"{Settings.ApiRoot}/v1/appInfos";

			var response = await _httpClient.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}

			var data = JsonSerializer.Deserialize<IEnumerable<AppInfoModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AppInfoModel>();

			return data;
		}
	}
}
