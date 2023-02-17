namespace PromoWeb.Api.Links;

using AutoMapper;
using PromoWeb.Services.Links;

public class LinkResponse
{
    public int Id { get; set; }

    public string LinkText { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int SectionId { get; set; }
    public string Section { get; set; } = string.Empty;
}

public class LinkResponseProfile : Profile
{
    public LinkResponseProfile()
    {
        CreateMap<LinkModel, LinkResponse>();
    }
}
