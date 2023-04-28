namespace PromoWeb.Api.Images;

using AutoMapper;
using PromoWeb.Context.Entities;
using PromoWeb.Services.Images;

public class ImageResponse
{
	/// <summary>
	/// Image id
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Image name
	/// </summary>
	public string ImageName { get; set; } = string.Empty;

	/// <summary>
	/// Image description
	/// </summary>
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Image file name for saving in FS
	/// </summary>
	public string UniqueName { get; set; }

	/// <summary>
	/// Info block that the photo belongs to
	/// </summary>
	public int AppInfoId { get; set; }

	/// <summary>
	/// Info block that the photo belongs to
	/// </summary>
	public string AppInfo { get; set; } = string.Empty;
}

public class ImageResponseProfile : Profile
{
    public ImageResponseProfile()
    {
        CreateMap<ImageModel, ImageResponse>();
    }
}
