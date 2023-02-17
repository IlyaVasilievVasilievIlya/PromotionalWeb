namespace PromoWeb.Services.AppInfos;

using AutoMapper;
using PromoWeb.Context.Entities;

public class AppInfoModel
{
    public int Id { get; set; }
    public string TextTitle { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;

    public int SectionId { get; set; }
    public string Section { get; set; } = string.Empty;
}

public class AppInfoModelProfile : Profile
{
    public AppInfoModelProfile()
    {
        CreateMap<AppInfo, AppInfoModel>()
            .ForMember(dst => dst.Section, a => a.MapFrom(src => src.Section.SectionName));
    }
}
