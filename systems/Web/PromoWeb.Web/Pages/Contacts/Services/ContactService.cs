using System.Text.Json;
using System.Text;

namespace PromoWeb.Web
{
    public class ContactService : IContactService
	{
		private readonly HttpClient _httpClient;

		public ContactService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<IEnumerable<ContactListItem>> GetContacts(int offset = 0, int limit = 10)
		{
			string url = $"{Settings.ApiRoot}/v1/contacts?offset={offset}&limit={limit}";

			var response = await _httpClient.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}

			var data = JsonSerializer.Deserialize<IEnumerable<ContactListItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ContactListItem>();

			return data;
		}

		public async Task<ContactListItem> GetContact(int contactId)
		{
			string url = $"{Settings.ApiRoot}/v1/contacts/{contactId}";

			var response = await _httpClient.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}

			var data = JsonSerializer.Deserialize<ContactListItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new ContactListItem();

			return data;
		}

		public async Task AddContact(ContactModel model)
		{
			string url = $"{Settings.ApiRoot}/v1/contacts";

			var body = JsonSerializer.Serialize(model);
			var request = new StringContent(body, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(url, request);

			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}

		public async Task EditContact(int contactId, ContactModel model)
		{
			string url = $"{Settings.ApiRoot}/v1/contacts/{contactId}";

			var body = JsonSerializer.Serialize(model);
			var request = new StringContent(body, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(url, request);

			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}

		public async Task DeleteContact(int contactId)
		{
			string url = $"{Settings.ApiRoot}/v1/contacts/{contactId}";

			var response = await _httpClient.DeleteAsync(url);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(content);
			}
		}
	}
}
