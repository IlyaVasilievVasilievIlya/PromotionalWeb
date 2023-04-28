namespace PromoWeb.Api.Contacts;

using AutoMapper;
using PromoWeb.Context.Entities;
using PromoWeb.Services.Contacts;

public class ContactResponse
{
	/// <summary>
	/// Contact id
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Contact owner
	/// </summary>
	public string ContactOwner { get; set; } = string.Empty;

	/// <summary>
	/// Contact email
	/// </summary>
	public string Email { get; set; } = string.Empty;

	/// <summary>
	/// Contact address
	/// </summary>
	public string Address { get; set; } = string.Empty;

	/// <summary>
	/// Contact website
	/// </summary>
	public string WebSite { get; set; } = string.Empty;

	/// <summary>
	/// Contact phone
	/// </summary>
	public string Phone { get; set; } = string.Empty;
}

public class ContactResponseProfile : Profile
{
    public ContactResponseProfile()
    {
        CreateMap<ContactModel, ContactResponse>();
    }
}
