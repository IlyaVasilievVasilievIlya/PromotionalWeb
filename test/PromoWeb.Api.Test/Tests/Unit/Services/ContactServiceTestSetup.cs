using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using PromoWeb.Common.Validator;
using PromoWeb.Context;
using PromoWeb.Context.Entities;
using PromoWeb.Services.Contacts;
using System.Numerics;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	public partial class ContactServiceTest
	{
		SqliteConnection connection;
		Mock<IDbContextFactory<MainDbContext>> factoryMock;
		Mock<IMapper> mapperMock;
		Mock<IModelValidator<UpdateContactModel>> updateValidatorMock;
		Mock<IModelValidator<AddContactModel>> addValidatorMock;
		ContactService service;
		DbContextOptions<MainDbContext> options;

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			connection = new SqliteConnection("DataSource=:memory:");

			options = new DbContextOptionsBuilder<MainDbContext>()
				.UseSqlite(connection)
				.Options;

			connection.Open();


			factoryMock = new Mock<IDbContextFactory<MainDbContext>>();
			factoryMock.Setup(factory => factory.CreateDbContextAsync(default)).Returns(async () => await Task.Run(() => new MainDbContext(options)));

			mapperMock = new Mock<IMapper>();
			mapperMock.Setup(mapper => mapper.Map<ContactModel?>(It.IsAny<Contact>()))
				.Returns((Contact? contact) => (contact is null) ? null 
				: new ContactModel 
				{ 
					Id = contact!.Id, 
					ContactOwner = contact.ContactOwner,
					Email = contact.Email ?? string.Empty,
					Address = contact.Address ?? string.Empty,
					WebSite = contact.WebSite ?? string.Empty,
					Phone = contact.Phone ?? string.Empty
				});

			mapperMock.Setup(mapper => mapper.Map<Contact>(It.IsAny<AddContactModel>()))
				.Returns((AddContactModel addModel) => 
				new Contact 
				{
					ContactOwner = addModel.ContactOwner,
					Email = addModel.Email,
					Address = addModel.Address,
					WebSite = addModel.WebSite,
					Phone = addModel.Phone
				});

			mapperMock.Setup(mapper => mapper.Map(It.IsAny<UpdateContactModel>(), It.IsAny<Contact>()))
				.Returns((UpdateContactModel updateModel, Contact contact) =>
				{
					contact.ContactOwner = updateModel.ContactOwner;
					contact.Email = updateModel.Email;
					contact.Address = updateModel.Address;
					contact.WebSite = updateModel.WebSite;
					contact.Phone = updateModel.Phone;
					return contact;
				});

			updateValidatorMock = new Mock<IModelValidator<UpdateContactModel>>();
			updateValidatorMock.Setup(model => model.Check(It.IsAny<UpdateContactModel>()));

			addValidatorMock = new Mock<IModelValidator<AddContactModel>>();
			addValidatorMock.Setup(model => model.Check(It.IsAny<AddContactModel>()));
		}


		[SetUp]
		public void Setup()
		{
			using (var context = new MainDbContext(options))
			{
				context.Database.EnsureCreated();
				context.Contacts.AddRange(
				new Contact { Id = 1, ContactOwner = "Owner1", Email = "contact1@tst.com", Address = "address1", WebSite = "site1.com", Phone = "1111" },
				new Contact { Id = 2, ContactOwner = "Owner2", Email = "contact2@tst.com", Address = "address2", WebSite = "site2.com", Phone = "2222" });
				context.SaveChanges();
			}
			service = new ContactService(factoryMock.Object, mapperMock.Object, addValidatorMock.Object, updateValidatorMock.Object);
		}

		[TearDown]
		public void ClearDb()
		{
			using var context = new MainDbContext(options);
			context.Contacts.RemoveRange(context.Contacts);
			context.SaveChanges();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			connection.Dispose();
		}
	}
}
