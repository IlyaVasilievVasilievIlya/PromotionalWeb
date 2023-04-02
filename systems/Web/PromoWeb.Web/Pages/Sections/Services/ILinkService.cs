namespace PromoWeb.Web
{
	public interface ILinkService
	{
		Task<IEnumerable<LinkListItem>> GetLinks(int offset = 0, int limit = 10);
		Task<LinkListItem> GetLink(int linkId);
		Task<IEnumerable<LinkListItem>> GetLinksBySectionId(int sectionId);
		Task AddLink(LinkModel request);
		Task EditLink(int linkId, LinkModel request);
		Task DeleteLink(int linkId);

	}
}
