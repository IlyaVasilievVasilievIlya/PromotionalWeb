using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace PromoWeb.Web
{
	public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;

        public ImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ImageListItem>> GetImages(int offset = 0, int limit = 10)
        {
            string url = $"{Settings.ApiRoot}/v1/images?offset={offset}&limit={limit}";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            var data = JsonSerializer.Deserialize<IEnumerable<ImageListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ImageListItem>();

            return data;
        }

        public async Task<IEnumerable<ImageListItem>> GetImagesByAppInfoId(int appInfoId)
        {
            string url = $"{Settings.ApiRoot}/v1/images/byAppInfo/{appInfoId}";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            var data = JsonSerializer.Deserialize<IEnumerable<ImageListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ImageListItem>();

            return data;
        }


        public async Task<ImageListItem> GetImage(int imageId)
        {
            string url = $"{Settings.ApiRoot}/v1/images/{imageId}";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            var data = JsonSerializer.Deserialize<ImageListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new ImageListItem();

            return data;
        }

        public async Task AddImage(ImageModel model)
        {
            string url = $"{Settings.ApiRoot}/v1/images";

            IBrowserFile file = model.Image;

            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream(1024 * 1024 * 50));

			fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

			content.Add(fileContent, "Image", file.Name);
            content.Add(new StringContent(model.AppInfoId.ToString(), Encoding.UTF8, "application/json"), "AppInfoId");
            content.Add(new StringContent(model.Description, Encoding.UTF8, "application/json"), "Description");
            content.Add(new StringContent(model.ImageName, Encoding.UTF8, "application/json"), "ImageName");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content.ToString());
            }
        }

        public async Task EditImage(int imageId, ImageModel model)
        {
            string url = $"{Settings.ApiRoot}/v1/images/{imageId}";

            var body = JsonSerializer.Serialize(model);
            var request = new StringContent(body, Encoding.UTF8, "application/json");


            IBrowserFile file = model.Image;

            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream(1024 * 1024 * 50));

            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

			content.Add(fileContent, "Image", file.Name);
			content.Add(new StringContent(model.AppInfoId.ToString(), Encoding.UTF8, "application/json"), "AppInfoId");
			content.Add(new StringContent(model.Description, Encoding.UTF8, "application/json"), "Description");
			content.Add(new StringContent(model.ImageName, Encoding.UTF8, "application/json"), "ImageName");

			var response = await _httpClient.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content.ToString());
            }
        }

        public async Task DeleteImage(int imageId)
        {
            string url = $"{Settings.ApiRoot}/v1/images/{imageId}";

            var response = await _httpClient.DeleteAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
        }

    }
}
