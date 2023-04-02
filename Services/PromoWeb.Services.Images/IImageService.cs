namespace PromoWeb.Services.Images;

public interface IImageService
{
    Task<IEnumerable<ImageModel>> GetImages(int offset = 0, int limit = 10);
    Task<IEnumerable<ImageModel>> GetImagesByAppInfoId(int appInfoId);
    Task<ImageModel> GetImage(int imageId);
    Task<ImageModel> AddImage(AddImageModel model);
    Task UpdateImage(int id, UpdateImageModel model);
    Task DeleteImage(int imageId);
}