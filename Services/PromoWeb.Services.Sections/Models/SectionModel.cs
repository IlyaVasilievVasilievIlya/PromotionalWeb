namespace PromoWeb.Services.Sections;

using AutoMapper;
using PromoWeb.Context.Entities;

public class SectionModel
{
    public int Id { get; set; }

    public string SectionName { get; set; } = string.Empty;
}

public class SectionModelProfile : Profile
{
    public SectionModelProfile()
    {
        CreateMap<Section, SectionModel>();
    }
}
