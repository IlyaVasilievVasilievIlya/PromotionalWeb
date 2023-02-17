namespace PromoWeb.Services.Contacts;

public interface IContactService
{
    Task<IEnumerable<ContactModel>> GetContacts(int offset = 0, int limit = 10);
    Task<ContactModel> GetContact(int contactId);
    Task<ContactModel> AddContact(AddContactModel model);
    Task UpdateContact(int id, UpdateContactModel model);
    Task DeleteContact(int contactId);
}