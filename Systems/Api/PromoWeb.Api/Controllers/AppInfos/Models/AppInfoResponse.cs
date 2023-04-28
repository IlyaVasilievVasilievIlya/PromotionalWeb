namespace PromoWeb.Api.AppInfos;

using AutoMapper;
using PromoWeb.Context.Entities;
using PromoWeb.Services.AppInfos;

public class AppInfoResponse
{
	/// <summary>
	/// AppInfo id
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// AppInfo title
	/// </summary>
	public string TextTitle { get; set; } = string.Empty;

	/// <summary>
	/// AppInfo text
	/// </summary>
	public string Text { get; set; } = string.Empty;

	/// <summary>
	/// Section that the appinfo belongs to
	/// </summary>
	public int SectionId { get; set; }

	/// <summary>
	/// Section that the appinfo belongs to
	/// </summary>
	public string Section { get; set; } = string.Empty;
}

public class AppInfoResponseProfile : Profile
{
    public AppInfoResponseProfile()
    {
        CreateMap<AppInfoModel, AppInfoResponse>();
    }
}
