using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using PromoWeb.Common.Validator;
using PromoWeb.Context.Entities;
using PromoWeb.Context;
using PromoWeb.Services.Images;
using PromoWeb.Services.AppInfos;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	public partial class AppInfoServiceTest
	{
		SqliteConnection connection;
		Mock<IDbContextFactory<MainDbContext>> factoryMock;
		Mock<IMapper> mapperMock;
		Mock<IModelValidator<UpdateAppInfoModel>> updateValidatorMock;
		Mock<IModelValidator<AddAppInfoModel>> addValidatorMock;
		AppInfoService service;
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
			mapperMock.Setup(mapper => mapper.Map<AppInfoModel?>(It.IsAny<AppInfo>()))
				.Returns((AppInfo? appInfo) => (appInfo is null) ? null
					 : new AppInfoModel
					 {
						 Id = appInfo!.Id,
						 TextTitle = appInfo.TextTitle,
						 Text = appInfo.Text,
						 SectionId = appInfo.SectionId,
						 Section = appInfo.Section?.SectionName ?? string.Empty
					 });

			mapperMock.Setup(mapper => mapper.Map<AppInfo>(It.IsAny<AddAppInfoModel>()))
				.Returns((AddAppInfoModel model) =>
					new AppInfo
					{
						TextTitle = model.TextTitle,
						Text = model.Text,
						SectionId = model.SectionId
					});

			mapperMock.Setup(mapper => mapper.Map(It.IsAny<UpdateAppInfoModel>(), It.IsAny<AppInfo>()))
				.Returns((UpdateAppInfoModel model, AppInfo appInfo) =>
				{
					appInfo.TextTitle = model!.TextTitle;
					appInfo.Text = model.Text;
					appInfo.SectionId = model.SectionId;
					return appInfo;
				});

			updateValidatorMock = new Mock<IModelValidator<UpdateAppInfoModel>>();
			updateValidatorMock.Setup(model => model.Check(It.IsAny<UpdateAppInfoModel>()));

			addValidatorMock = new Mock<IModelValidator<AddAppInfoModel>>();
			addValidatorMock.Setup(model => model.Check(It.IsAny<AddAppInfoModel>()));
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
				context.AppInfos.AddRange(
					new AppInfo { Id = 1, TextTitle = "Title1", Text = "Text1", SectionId = 1 },
					new AppInfo { Id = 2, TextTitle = "Title2", Text = "Text2", SectionId = 1 });

				context.SaveChanges();
			}
			service = new AppInfoService(factoryMock.Object, mapperMock.Object, addValidatorMock.Object, updateValidatorMock.Object);
		}

		[TearDown]
		public void ClearDb()
		{
			using var context = new MainDbContext(options);
			context.AppInfos.RemoveRange(context.AppInfos);
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
