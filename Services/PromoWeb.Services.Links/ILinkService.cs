namespace PromoWeb.Services.Links;

public interface ILinkService
{
    Task<IEnumerable<LinkModel>> GetLinks(int offset = 0, int limit = 10);
    Task<LinkModel> GetLink(int linkId);
    Task<LinkModel> AddLink(AddLinkModel model);
    Task UpdateLink(int id, UpdateLinkModel model);
    Task DeleteLink(int linkId);
}