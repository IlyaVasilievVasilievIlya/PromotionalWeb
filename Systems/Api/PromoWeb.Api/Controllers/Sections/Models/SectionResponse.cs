namespace PromoWeb.Api.Sections;

using AutoMapper;
using PromoWeb.Services.Sections;

public class SectionResponse
{   
    /// <summary>
	/// Section id
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Section name
	/// </summary>
	public string SectionName { get; set; } = string.Empty;
}

public class SectionResponseProfile : Profile
{
    public SectionResponseProfile()
    {
        CreateMap<SectionModel, SectionResponse>();
    }
}
