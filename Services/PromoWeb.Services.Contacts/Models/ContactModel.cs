namespace PromoWeb.Services.Contacts;

using AutoMapper;
using PromoWeb.Context.Entities;

public class ContactModel
{
    public int Id { get; set; }

    public string ContactOwner { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? WebSite { get; set; }
    public string? Phone { get; set; }
}

public class ContactModelProfile : Profile
{
    public ContactModelProfile()
    {
        CreateMap<Contact, ContactModel>();
    }
}
