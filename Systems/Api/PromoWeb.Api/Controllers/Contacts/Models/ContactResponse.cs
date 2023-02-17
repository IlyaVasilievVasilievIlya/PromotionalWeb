namespace PromoWeb.Api.Contacts;

using AutoMapper;
using PromoWeb.Context.Entities;
using PromoWeb.Services.Contacts;

public class ContactResponse
{
    public int Id { get; set; }

    public string ContactOwner { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? WebSite { get; set; }
    public string? Phone { get; set; }
}

public class ContactResponseProfile : Profile
{
    public ContactResponseProfile()
    {
        CreateMap<ContactModel, ContactResponse>();
    }
}
