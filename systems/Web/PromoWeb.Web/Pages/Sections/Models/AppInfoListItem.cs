namespace PromoWeb.Web
{
	public class AppInfoListItem
	{
		public bool ShowDetails { get; set; }
		public IEnumerable<ImageListItem> Images { get; set; } 


		public int Id { get; set; }


		public string TextTitle { get; set; } = string.Empty;
		public string Text { get; set; } = string.Empty;
	}
}
