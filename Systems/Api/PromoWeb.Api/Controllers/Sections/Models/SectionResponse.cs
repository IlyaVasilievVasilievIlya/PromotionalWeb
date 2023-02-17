namespace PromoWeb.Api.Sections;

using AutoMapper;
using PromoWeb.Services.Sections;

public class SectionResponse
{
    public int Id { get; set; }

    public string SectionName { get; set; } = string.Empty;
}

public class SectionResponseProfile : Profile
{
    public SectionResponseProfile()
    {
        CreateMap<SectionModel, SectionResponse>();
    }
}
