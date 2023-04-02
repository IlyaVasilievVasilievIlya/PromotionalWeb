namespace PromoWeb.Web
{
    public interface ISectionService
    {
        Task<IEnumerable<SectionListItem>> GetSections(int offset = 0, int limit = 10);
        Task<SectionListItem> GetSection(int sectionId);
        Task AddSection(SectionModel model);
        Task EditSection(int sectionId, SectionModel model);
        Task DeleteSection(int sectionId);

        Task<IEnumerable<AppInfoModel>> GetAppInfoList();
        Task<IEnumerable<LinkModel>> GetLinkList();
    }
}
