namespace PromoWeb.Services.Links;

using AutoMapper;
using PromoWeb.Context.Entities;

public class LinkModel
{
    public int Id { get; set; }

    public string LinkText { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int SectionId { get; set; }
    public string Section { get; set; } = string.Empty;
}

public class LinkModelProfile : Profile
{
    public LinkModelProfile()
    {
        CreateMap<Link, LinkModel>()
            .ForMember(dest => dest.Section, opt => opt.MapFrom(src => src.Section.SectionName));
    }
}
