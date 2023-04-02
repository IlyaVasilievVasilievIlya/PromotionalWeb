namespace PromoWeb.Web
{
	public interface IImageService
	{
        Task<IEnumerable<ImageListItem>> GetImages(int offset = 0, int limit = 10);
        Task<ImageListItem> GetImage(int imageId);
        Task<IEnumerable<ImageListItem>> GetImagesByAppInfoId(int appInfoId);
        Task AddImage(ImageModel request);
        Task EditImage(int imageId, ImageModel request);
        Task DeleteImage(int imageId);
    }
}
