namespace PromoWeb.Api.AppInfos;

using AutoMapper;
using PromoWeb.Context.Entities;
using PromoWeb.Services.AppInfos;

public class AppInfoResponse
{
    public int Id { get; set; }

    public string TextTitle { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;

    public int SectionId { get; set; }
    public string Section { get; set; } = string.Empty;
}

public class AppInfoResponseProfile : Profile
{
    public AppInfoResponseProfile()
    {
        CreateMap<AppInfoModel, AppInfoResponse>();
    }
}
