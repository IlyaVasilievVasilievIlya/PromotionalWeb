using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using PromoWeb.Common.Validator;
using PromoWeb.Context;
using PromoWeb.Context.Entities;
using PromoWeb.Services.Images;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	public partial class ImageServiceTest
	{
		SqliteConnection connection;
		Mock<IDbContextFactory<MainDbContext>> factoryMock;
		Mock<IMapper> mapperMock;
		Mock<IModelValidator<UpdateImageModel>> updateValidatorMock;
		Mock<IModelValidator<AddImageModel>> addValidatorMock;
		ImageService service;
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
			mapperMock.Setup(mapper => mapper.Map<ImageModel?>(It.IsAny<Image>()))
				.Returns((Image? image) => (image is null) ? null
					 : new ImageModel
					 {
						 Id = image!.Id,
						 Description = image!.Description,
						 ImageName = image.ImageName,
						 AppInfoId = image.AppInfoId,
						 AppInfo = image.AppInfo?.TextTitle,
						 UniqueName = image.UniqueName
					 });

			mapperMock.Setup(mapper => mapper.Map<Image>(It.IsAny<AddImageModel>()))
				.Returns((AddImageModel model) =>
					new Image
					{
						Description = model!.Description,
						ImageName = model.ImageName,
						AppInfoId = model.AppInfoId,
						UniqueName = model.UniqueName
					});

			mapperMock.Setup(mapper => mapper.Map(It.IsAny<UpdateImageModel>(), It.IsAny<Image>()))
				.Returns((UpdateImageModel model, Image image) =>
				{
					image.Description = model!.Description;
					image.ImageName = model.ImageName;
					image.AppInfoId = model.AppInfoId;
					image.UniqueName = model.UniqueName;
					return image;
				});

			updateValidatorMock = new Mock<IModelValidator<UpdateImageModel>>();
			updateValidatorMock.Setup(model => model.Check(It.IsAny<UpdateImageModel>()));

			addValidatorMock = new Mock<IModelValidator<AddImageModel>>();
			addValidatorMock.Setup(model => model.Check(It.IsAny<AddImageModel>()));

			using var context = new MainDbContext(options);
			context.Database.EnsureCreated();
			var section = new Section { Id = 1, SectionName = "Section1" };
			context.Sections.Add(section);
			var appinfo = new AppInfo() { Id = 1, Section = section, TextTitle = "TT1", Text = "T1" };
			context.AppInfos.Add(appinfo);
			context.SaveChanges();
		}


		[SetUp]
		public void Setup()
		{
			using (var context = new MainDbContext(options))
			{
				context.Database.EnsureCreated();
				context.Images.AddRange(
					new Image { Id = 1, ImageName = "Image1", Description = "desc1", UniqueName = "unique1", AppInfoId = 1 },
					new Image { Id = 2, ImageName = "Image2", Description = "desc2", UniqueName = "unique2", AppInfoId = 1 } );

				context.SaveChanges();
			}
			service = new ImageService(factoryMock.Object, mapperMock.Object, addValidatorMock.Object, updateValidatorMock.Object);
		}

		[TearDown]
		public void ClearDb()
		{
			using var context = new MainDbContext(options);
			context.Images.RemoveRange(context.Images);
			context.SaveChanges();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			connection.Dispose();
		}
	}
}
