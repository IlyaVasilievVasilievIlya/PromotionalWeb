namespace PromoWeb.Api.Links;

using AutoMapper;
using PromoWeb.Services.Links;

public class LinkResponse
{
	/// <summary>
	/// Link id
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Link
	/// </summary>
	public string LinkText { get; set; } = string.Empty;

	/// <summary>
	/// Link description
	/// </summary>
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Section that the link belongs to
	/// </summary>
	public int SectionId { get; set; }

	/// <summary>
	/// Section that the link belongs to
	/// </summary>
	public string Section { get; set; } = string.Empty;
}

public class LinkResponseProfile : Profile
{
    public LinkResponseProfile()
    {
        CreateMap<LinkModel, LinkResponse>();
    }
}
