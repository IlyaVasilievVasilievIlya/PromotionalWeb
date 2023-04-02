namespace PromoWeb.Web
{
    public interface IContactService
    {
		Task<IEnumerable<ContactListItem>> GetContacts(int offset = 0, int limit = 10);
		Task<ContactListItem> GetContact(int contactId);
		Task AddContact(ContactModel model);
		Task EditContact(int contactId, ContactModel model);
		Task DeleteContact(int contactId);
	}
}
