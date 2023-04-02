namespace PromoWeb.Api.Contacts;

using AutoMapper;
using PromoWeb.Context.Entities;
using PromoWeb.Services.Contacts;

public class ContactResponse
{
    public int Id { get; set; }

    public string ContactOwner { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string WebSite { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
}

public class ContactResponseProfile : Profile
{
    public ContactResponseProfile()
    {
        CreateMap<ContactModel, ContactResponse>();
    }
}
