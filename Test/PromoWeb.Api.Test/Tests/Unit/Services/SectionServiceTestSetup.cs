using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using PromoWeb.Common.Validator;
using PromoWeb.Context;
using PromoWeb.Context.Entities;
using PromoWeb.Services.Sections;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	public partial class SectionServiceTest
	{
		SqliteConnection connection;
		Mock<IDbContextFactory<MainDbContext>> factoryMock;
		Mock<IMapper> mapperMock;
		Mock<IModelValidator<UpdateSectionModel>> updateValidatorMock;
		Mock<IModelValidator<AddSectionModel>> addValidatorMock;
		SectionService service;
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
			mapperMock.Setup(mapper => mapper.Map<SectionModel?>(It.IsAny<Section>()))
				.Returns((Section? section) => (section is null) ? null : new SectionModel { Id = section!.Id, SectionName = section.SectionName });

			mapperMock.Setup(mapper => mapper.Map<Section>(It.IsAny<AddSectionModel>()))
				.Returns((AddSectionModel newSection) => new Section { SectionName = newSection.SectionName });

			mapperMock.Setup(mapper => mapper.Map(It.IsAny<UpdateSectionModel>(), It.IsAny<Section>()))
				.Returns((UpdateSectionModel newSection, Section section) =>
					{
						section.SectionName = newSection.SectionName; 
						return section; 
					});

			updateValidatorMock = new Mock<IModelValidator<UpdateSectionModel>>();
			updateValidatorMock.Setup(model => model.Check(It.IsAny<UpdateSectionModel>()));

			addValidatorMock = new Mock<IModelValidator<AddSectionModel>>();
			addValidatorMock.Setup(model => model.Check(It.IsAny<AddSectionModel>()));
		}


		[SetUp]
		public void Setup()
		{
			using (var context = new MainDbContext(options))
			{
				context.Database.EnsureCreated();
				context.Sections.AddRange(
				new Section { Id = 1, SectionName = "Section1" },
				new Section { Id = 2, SectionName = "Section2" });
				context.SaveChanges();
			}
			service = new SectionService(factoryMock.Object, mapperMock.Object, addValidatorMock.Object, updateValidatorMock.Object);
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
