using System.Text.Json;
using System.Text;

namespace PromoWeb.Web
{
    public class AppInfoService : IAppInfoService
    {
        private readonly HttpClient _httpClient;

        public AppInfoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AppInfoListItem>> GetAppInfos(int offset = 0, int limit = 10)
        {
            string url = $"{Settings.ApiRoot}/v1/appInfos?offset={offset}&limit={limit}";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            var data = JsonSerializer.Deserialize<IEnumerable<AppInfoListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AppInfoListItem>();

            return data;
        }

        public async Task<IEnumerable<AppInfoListItem>> GetAppInfosBySectionId(int sectionId)
        {
            string url = $"{Settings.ApiRoot}/v1/appInfos/bySection/{sectionId}";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            var data = JsonSerializer.Deserialize<IEnumerable<AppInfoListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AppInfoListItem>();

            return data;
        }

        public async Task<AppInfoListItem> GetAppInfo(int appInfoId)
        {
            string url = $"{Settings.ApiRoot}/v1/appInfos/{appInfoId}";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            var data = JsonSerializer.Deserialize<AppInfoListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new AppInfoListItem();

            return data;
        }

        public async Task AddAppInfo(AppInfoModel model)
        {
            string url = $"{Settings.ApiRoot}/v1/appInfos";

            var body = JsonSerializer.Serialize(model);
            var request = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, request);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
        }

        public async Task EditAppInfo(int appInfoId, AppInfoModel model)
        {
            string url = $"{Settings.ApiRoot}/v1/appInfos/{appInfoId}";

            var body = JsonSerializer.Serialize(model);
            var request = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, request);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
        }

        public async Task DeleteAppInfo(int appInfoId)
        {
            string url = $"{Settings.ApiRoot}/v1/appInfos/{appInfoId}";

            var response = await _httpClient.DeleteAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
        }
    }
}
