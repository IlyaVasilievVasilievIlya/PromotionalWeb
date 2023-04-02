namespace PromoWeb.Web
{
	public class ContactListItem
	{
		public int Id { get; set; }
		public string ContactOwner { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string WebSite { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
	}
}
