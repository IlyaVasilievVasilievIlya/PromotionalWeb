namespace PromoWeb.Services.Contacts;

using AutoMapper;
using PromoWeb.Context.Entities;

public class ContactModel
{
    public int Id { get; set; }

    public string ContactOwner { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string WebSite { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
}

public class ContactModelProfile : Profile
{
    public ContactModelProfile()
    {
        CreateMap<Contact, ContactModel>();
    }
}
