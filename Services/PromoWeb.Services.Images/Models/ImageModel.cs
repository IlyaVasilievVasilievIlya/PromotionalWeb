namespace PromoWeb.Services.Images;

using AutoMapper;
using PromoWeb.Context.Entities;

public class ImageModel
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;

    public string UniqueName { get; set; } = string.Empty;

    public int AppInfoId { get; set; }
    public string AppInfo { get; set; } = string.Empty;
}

public class ImageModelProfile : Profile
{
    public ImageModelProfile()
    {
        CreateMap<Image, ImageModel>()
            .ForMember(dest => dest.AppInfo, opt => opt.MapFrom(src => src.AppInfo.TextTitle));
    }
}
