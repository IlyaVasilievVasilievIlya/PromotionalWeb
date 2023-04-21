using FluentValidation;
using Moq;
using PromoWeb.Common.Exceptions;
using PromoWeb.Context;
using PromoWeb.Services.Contacts;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	public partial class ContactServiceTest
	{
		//Get
		[Test]
		public async Task GetContacts_ReturnsNotNull()
		{
			var models = await service.GetContacts();

			Assert.NotNull(models);
		}

		[Test]
		public async Task GetContacts_ReturnsExpectedType()
		{
			var models = await service.GetContacts();

			Assert.IsInstanceOf(typeof(IEnumerable<ContactModel>), models);
		}

		[Test]
		public async Task GetContacts_ReturnsExpectedValues()
		{
			var models = await service.GetContacts();

			Assert.That(models.Count, Is.EqualTo(2));
			Assert.That(models.ElementAt(0).Id, Is.EqualTo(1));
			Assert.That(models.ElementAt(1).ContactOwner, Is.EqualTo("Owner2"));
		}

		[Test]
		public async Task GetContactsWithOffset_ReturnsOneElement()
		{
			var models = await service.GetContacts(1);

			Assert.That(models.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task GetContactsWithInvalidOffset_ReturnsTwoElements()
		{
			var models = await service.GetContacts(-100);

			Assert.That(models.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task GetContactsWithOffsetLimit_ReturnsEmptyCollection()
		{
			var models = await service.GetContacts(10000000, 10000000);

			Assert.That(models.Count, Is.EqualTo(0));
		}


		//GetById
		[Test]
		public async Task GetContactById_ReturnsNotNull()
		{
			var model = await service.GetContact(1);

			Assert.NotNull(model);
		}

		[Test]
		public async Task GetContactByIdWithNonExistingId_ReturnsNull()
		{
			var model = await service.GetContact(500);

			Assert.IsNull(model);
		}

		[Test]
		public async Task GetContactById_ReturnsExpectedType()
		{
			var model = await service.GetContact(1);

			Assert.IsInstanceOf(typeof(ContactModel), model);
		}

		[Test]
		public async Task GetContactById_ReturnsExpectedValue()
		{
			var model = await service.GetContact(1);

			Assert.That(model.Id, Is.EqualTo(1));
			Assert.That(model.ContactOwner, Is.EqualTo("Owner1"));
			Assert.That(model.Email, Is.EqualTo("contact1@tst.com"));
			Assert.That(model.Address, Is.EqualTo("address1"));
			Assert.That(model.WebSite, Is.EqualTo("site1.com"));
			Assert.That(model.Phone, Is.EqualTo("1111"));
		}

		//Add
		[Test]
		public async Task AddContact_ReturnsNotNull()
		{
			AddContactModel addModel = new AddContactModel()
			{
				ContactOwner = "Owner3",
				Email = "contact3@tst.com",
				Address = "address3",
				WebSite = "site3.com",
				Phone = "3333"
			};

			var model = await service.AddContact(addModel);

			Assert.That(model, Is.Not.Null);
		}

		[Test]
		public async Task AddContact_ReturnsExpectedType()
		{
			AddContactModel addModel = new AddContactModel()
			{
				ContactOwner = "Owner3",
				Email = "contact3@tst.com",
				Address = "address3",
				WebSite = "site3.com",
				Phone = "3333"
			};

			var model = await service.AddContact(addModel);

			Assert.That(model, Is.TypeOf(typeof(ContactModel)));
		}

		[Test]
		public async Task AddContact_ReturnsExpectedValue()
		{
			AddContactModel addModel = new AddContactModel()
			{
				ContactOwner = "Owner3",
				Email = "contact3@tst.com",
				Address = "address3",
				WebSite = "site3.com",
				Phone = "3333"
			};

			var model = await service.AddContact(addModel);

			Assert.That(model.ContactOwner, Is.EqualTo(addModel.ContactOwner));
			Assert.That(model.Email, Is.EqualTo(addModel.Email));
			Assert.That(model.Address, Is.EqualTo(addModel.Address));
			Assert.That(model.WebSite, Is.EqualTo(addModel.WebSite));
			Assert.That(model.Phone, Is.EqualTo(addModel.Phone));
		}

		[Test]
		public async Task AddContact_DbContainsAddedElem()
		{
			AddContactModel addModel = new AddContactModel()
			{
				ContactOwner = "Owner3",
				Email = "contact3@tst.com",
				Address = "address3",
				WebSite = "site3.com",
				Phone = "3333"
			};

			await service.AddContact(addModel);
			using var context = new MainDbContext(options);

			Assert.That(context.Contacts.Any(elem => elem.ContactOwner == addModel.ContactOwner && elem.Email == addModel.Email
												&& elem.Address == addModel.Address && elem.WebSite == addModel.WebSite && elem.Phone == addModel.Phone), Is.True);
		}

		[Test]
		public void AddContact_ThrowsValidationException()
		{
			AddContactModel addModel = new AddContactModel()
			{
				ContactOwner = "Owner3",
				Email = "contact3@tst.com",
				Address = "address3",
				WebSite = "site3.com",
				Phone = "3333"
			};
			addValidatorMock.Setup(model => model.Check(It.IsAny<AddContactModel>())).Callback(() => throw new ValidationException("Error"));

			Assert.ThrowsAsync(typeof(ValidationException), async () => await service.AddContact(addModel));
		}


		//Update
		[Test]
		public async Task UpdateContact_DbContainsUpdatedElem()
		{
			UpdateContactModel updateModel = new UpdateContactModel() 
			{
				ContactOwner = "Owner3",
				Email = "contact3@tst.com",
				Address = "address3",
				WebSite = "site3.com",
				Phone = "3333"
			};

			await service.UpdateContact(2, updateModel);
			using var context = new MainDbContext(options);

			Assert.That(context.Contacts.Any(elem => elem.ContactOwner == updateModel.ContactOwner && elem.Email == updateModel.Email
												&& elem.Address == updateModel.Address 
												&& elem.WebSite == updateModel.WebSite && elem.Phone == updateModel.Phone), Is.True);
		}

		[Test]
		public void UpdateContact_ThrowsProcessExceptionNotFound()
		{
			UpdateContactModel updateModel = new UpdateContactModel()
			{
				ContactOwner = "Owner3",
				Email = "contact3@tst.com",
				Address = "address3",
				WebSite = "site3.com",
				Phone = "3333"
			};

			Assert.ThrowsAsync(typeof(ProcessException), async () => await service.UpdateContact(100, updateModel));
		}

		[Test]
		public void UpdateContact_ThrowsValidationException()
		{
			UpdateContactModel updateModel = new UpdateContactModel()
			{
				ContactOwner = "Owner3",
				Email = "contact3@tst.com",
				Address = "address3",
				WebSite = "site3.com",
				Phone = "3333"
			};
			updateValidatorMock.Setup(model => model.Check(It.IsAny<UpdateContactModel>())).Callback(() => throw new ValidationException("Error"));

			Assert.ThrowsAsync(typeof(ValidationException), async () => await service.UpdateContact(1, updateModel));
		}


		//Delete
		[Test]
		public async Task DeleteContact_DbDoNotContainDeletedElem()
		{
			await service.DeleteContact(1);
			using var context = new MainDbContext(options);

			Assert.That(context.Contacts.Any(elem => elem.Id == 1), Is.False);
		}

		[Test]
		public void DeleteContact_ThrowsProcessExceptionNotFound()
		{
			Assert.ThrowsAsync(typeof(ProcessException), async () => await service.DeleteContact(100));
		}
	}
}
