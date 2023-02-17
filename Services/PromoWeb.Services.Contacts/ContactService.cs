namespace PromoWeb.Services.Contacts;

using AutoMapper;
using PromoWeb.Common.Exceptions;
using PromoWeb.Common.Validator;
using PromoWeb.Context;
using PromoWeb.Context.Entities;
using Microsoft.EntityFrameworkCore;

public class ContactService : IContactService
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IMapper mapper;
    private readonly IModelValidator<AddContactModel> addContactModelValidator;
    private readonly IModelValidator<UpdateContactModel> updateContactModelValidator;

    public ContactService(
        IDbContextFactory<MainDbContext> contextFactory,
        IMapper mapper,
        IModelValidator<AddContactModel> addContactModelValidator,
        IModelValidator<UpdateContactModel> updateContactModelValidator
        )
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
        this.addContactModelValidator = addContactModelValidator;
        this.updateContactModelValidator = updateContactModelValidator;
    }

    public async Task<IEnumerable<ContactModel>> GetContacts(int offset = 0, int limit = 10)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var contacts = context
            .Contacts
            .AsQueryable();

        contacts = contacts
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));

        var data = (await contacts.ToListAsync()).Select(contact => mapper.Map<ContactModel>(contact));

        return data;
    }

    public async Task<ContactModel> GetContact(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var contact = await context.Contacts.FirstOrDefaultAsync(x => x.Id.Equals(id));

        var data = mapper.Map<ContactModel>(contact);

        return data;
    }
    public async Task<ContactModel> AddContact(AddContactModel model)
    {
        addContactModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var contact = mapper.Map<Contact>(model);
        await context.Contacts.AddAsync(contact);
        context.SaveChanges();

        return mapper.Map<ContactModel>(contact);
    }

    public async Task UpdateContact(int contactId, UpdateContactModel model)
    {
        updateContactModelValidator.Check(model);

        using var context = await contextFactory.CreateDbContextAsync();

        var contact = await context.Contacts.FirstOrDefaultAsync(x => x.Id.Equals(contactId));

        ProcessException.ThrowIf(() => contact is null, $"The contact (id: {contactId}) was not found");

        contact = mapper.Map(model, contact);

        context.Contacts.Update(contact);
        context.SaveChanges();
    }

    public async Task DeleteContact(int contactId)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var contact = await context.Contacts.FirstOrDefaultAsync(x => x.Id.Equals(contactId))
            ?? throw new ProcessException($"The contact (id: {contactId}) was not found");

        context.Remove(contact);
        context.SaveChanges();
    }
}
