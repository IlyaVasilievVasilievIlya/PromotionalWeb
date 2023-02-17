namespace PromoWeb.Services.AppInfos
{
    public interface IAppInfoService
    {
        Task<IEnumerable<AppInfoModel>> GetAppInfos(int offset = 0, int limit = 10);

        Task<AppInfoModel> GetAppInfo(int infoId);
        Task<AppInfoModel> AddAppInfo(AddAppInfoModel model);
        Task UpdateAppInfo(int id, UpdateAppInfoModel model);
        Task DeleteAppInfo(int infoId);
    }
}
