using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using PromoWeb.Common.Validator;
using PromoWeb.Context.Entities;
using PromoWeb.Context;
using PromoWeb.Services.Links;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	public partial class LinkServiceTest
	{
		SqliteConnection connection;
		Mock<IDbContextFactory<MainDbContext>> factoryMock;
		Mock<IMapper> mapperMock;
		Mock<IModelValidator<UpdateLinkModel>> updateValidatorMock;
		Mock<IModelValidator<AddLinkModel>> addValidatorMock;
		LinkService service;
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
			mapperMock.Setup(mapper => mapper.Map<LinkModel?>(It.IsAny<Link>()))
				.Returns((Link? link) => (link is null) ? null
					 : new LinkModel
					 {
						 Id = link!.Id,
						 Description = link!.Description,
						 LinkText = link.LinkText,
						 SectionId = link.SectionId,
						 Section = link.Section?.SectionName ?? string.Empty
					 });

			mapperMock.Setup(mapper => mapper.Map<Link>(It.IsAny<AddLinkModel>()))
				.Returns((AddLinkModel model) =>
					new Link
					{
						Description = model!.Description,
						LinkText = model.LinkText,
						SectionId = model.SectionId
					});

			mapperMock.Setup(mapper => mapper.Map(It.IsAny<UpdateLinkModel>(), It.IsAny<Link>()))
				.Returns((UpdateLinkModel model, Link link) =>
				{
					link.Description = model!.Description;
					link.LinkText = model.LinkText;
					link.SectionId = model.SectionId;
					return link;
				});

			updateValidatorMock = new Mock<IModelValidator<UpdateLinkModel>>();
			updateValidatorMock.Setup(model => model.Check(It.IsAny<UpdateLinkModel>()));

			addValidatorMock = new Mock<IModelValidator<AddLinkModel>>();
			addValidatorMock.Setup(model => model.Check(It.IsAny<AddLinkModel>()));
		}


		[SetUp]
		public void Setup()
		{
			using (var context = new MainDbContext(options))
			{
				context.Database.EnsureCreated();
				var sections = new Section[] 
					{ 
						new Section() { Id = 1, SectionName = "Section1" },
						new Section() { Id = 2, SectionName = "Section2" }
					};
				context.Sections.AddRange(sections);
				context.Links.AddRange(
					new Link { Id = 1, LinkText = "Link1", Description = "desc1", SectionId = 1 },
					new Link { Id = 2, LinkText = "Link2", Description = "desc2", SectionId = 1 });

				context.SaveChanges();
			}
			service = new LinkService(factoryMock.Object, mapperMock.Object, addValidatorMock.Object, updateValidatorMock.Object);
		}

		[TearDown]
		public void ClearDb()
		{
			using var context = new MainDbContext(options);
			context.Links.RemoveRange(context.Links);
			context.Sections.RemoveRange(context.Sections);
			context.SaveChanges();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			connection.Dispose();
		}
	}
}
