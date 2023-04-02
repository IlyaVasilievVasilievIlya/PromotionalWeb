namespace PromoWeb.Web
{
	public interface IAppInfoService
	{
        Task<IEnumerable<AppInfoListItem>> GetAppInfos(int offset = 0, int limit = 10);
        Task<AppInfoListItem> GetAppInfo(int contactId);
        Task<IEnumerable<AppInfoListItem>> GetAppInfosBySectionId(int sectionId);

        Task AddAppInfo(AppInfoModel model);
        Task EditAppInfo(int appInfoId, AppInfoModel model);
        Task DeleteAppInfo(int appInfoId);
    }
}
