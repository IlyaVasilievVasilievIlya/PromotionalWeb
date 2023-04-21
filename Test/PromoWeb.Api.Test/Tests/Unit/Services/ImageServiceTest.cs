using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using PromoWeb.Common.Exceptions;
using PromoWeb.Context;
using PromoWeb.Services.Images;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	[TestFixture]
	public partial class ImageServiceTest
	{
		//Get
		[Test]
		public async Task GetImages_ReturnsNotNull()
		{
			var models = await service.GetImages();

			Assert.NotNull(models);
		}

		[Test]
		public async Task GetImages_ReturnsExpectedType()
		{
			var models = await service.GetImages();

			Assert.IsInstanceOf(typeof(IEnumerable<ImageModel>), models);
		}

		[Test]
		public async Task GetImages_ReturnsExpectedValues()
		{
			var models = await service.GetImages();

			Assert.That(models.Count(), Is.EqualTo(2));
			Assert.That(models.ElementAt(0).ImageName, Is.EqualTo("Image1"));
			Assert.That(models.ElementAt(1).Id, Is.EqualTo(2));
			Assert.That(models.ElementAt(0).AppInfo, Is.EqualTo("TT1"));
		}

		[Test]
		public async Task GetImagesWithOffset_ReturnsOneElement()
		{
			var models = await service.GetImages(1);

			Assert.That(models.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task GetImagesWithInvalidOffset_ReturnsTwoElements()
		{
			var models = await service.GetImages(-100);

			Assert.That(models.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task GetImagesWithOffsetLimit_ReturnsEmptyCollection()
		{
			var models = await service.GetImages(10000000, 10000000);

			Assert.That(models.Count, Is.EqualTo(0));
		}

		//GetByAppInfoId
		[Test]
		public async Task GetImagesByAppInfoId_ReturnsNotNull()
		{
			var models = await service.GetImagesByAppInfoId(1);

			Assert.NotNull(models);
		}

		[Test]
		public async Task GetImagesByAppInfoId_ReturnsExpectedType()
		{
			var models = await service.GetImagesByAppInfoId(1);

			Assert.IsInstanceOf(typeof(IEnumerable<ImageModel>), models);
		}

		[Test]
		public async Task GetImagesByAppInfoId_ReturnsValuesWithSameAppInfo()
		{
			var models = await service.GetImagesByAppInfoId(1);

			Assert.That(models.Count(), Is.EqualTo(2));
			Assert.That(models.ElementAt(0).ImageName, Is.EqualTo("Image1"));
			Assert.That(models.ElementAt(1).Id, Is.EqualTo(2));
			Assert.That(models, Has.All.With.Property("AppInfoId").EqualTo(1));
		}

		//GetById
		[Test]
		public async Task GetImageById_ReturnsNotNull()
		{
			var model = await service.GetImage(1);

			Assert.NotNull(model);
		}

		[Test]
		public async Task GetImageByIdWithNonExistingId_ReturnsNull()
		{
			var model = await service.GetImage(500);

			Assert.IsNull(model);
		}

		[Test]
		public async Task GetImageById_ReturnsExpectedType()
		{
			var model = await service.GetImage(1);

			Assert.IsInstanceOf(typeof(ImageModel), model);
		}

		[Test]
		public async Task GetImageById_ReturnsExpectedValue()
		{
			var model = await service.GetImage(1);

			Assert.That(model.Id, Is.EqualTo(1));
			Assert.That(model.ImageName, Is.EqualTo("Image1"));
			Assert.That(model.AppInfo, Is.EqualTo("TT1"));
		}

		//Add
		[Test]
		public async Task AddImage_ReturnsNotNull()
		{
			AddImageModel addModel = new AddImageModel() { ImageName = "Image3", Description = "desc3", UniqueName = "unique3", AppInfoId = 1 };

			var model = await service.AddImage(addModel);

			Assert.That(model, Is.Not.Null);
		}

		[Test]
		public async Task AddImage_ReturnsExpectedType()
		{
			AddImageModel addModel = new AddImageModel() { ImageName = "Image3", Description = "desc3", UniqueName = "unique3", AppInfoId = 1 };

			var model = await service.AddImage(addModel);

			Assert.That(model, Is.TypeOf(typeof(ImageModel)));
		}

		[Test]
		public async Task AddImage_ReturnsExpectedValue()
		{
			AddImageModel addModel = new AddImageModel() { ImageName = "Image3", Description = "desc3", UniqueName = "unique3", AppInfoId = 1 };

			var model = await service.AddImage(addModel);

			Assert.That(model.ImageName, Is.EqualTo(addModel.ImageName)); 
			Assert.That(model.Description, Is.EqualTo(addModel.Description));
			Assert.That(model.UniqueName, Is.EqualTo(addModel.UniqueName));
			Assert.That(model.AppInfoId, Is.EqualTo(1));

		}

		[Test]
		public async Task AddImage_DbContainsAddedElem()
		{
			AddImageModel addModel = new AddImageModel() { ImageName = "Image3", Description = "desc3", UniqueName = "unique3", AppInfoId = 1 };

			await service.AddImage(addModel);
			using var context = new MainDbContext(options);

			Assert.That(context.Images.Any(elem => elem.ImageName == addModel.ImageName && elem.Description == addModel.Description
													&& (elem.AppInfo != null) && elem.UniqueName == addModel.UniqueName), Is.True);
		}

		[Test]
		public void AddImage_ThrowsValidationException()
		{
			AddImageModel addModel = new AddImageModel() { ImageName = "Image3", Description = "desc3", UniqueName = "unique3", AppInfoId = 1 };

			addValidatorMock.Setup(model => model.Check(It.IsAny<AddImageModel>())).Callback(() => throw new ValidationException("Error"));

			Assert.ThrowsAsync(typeof(ValidationException), async () => await service.AddImage(addModel));
		}

		[Test]
		public void AddImage_ModelWithNonExistingForeighKey_ThrowsException()
		{
			AddImageModel addModel = new AddImageModel() { ImageName = "Image3", Description = "desc3", UniqueName = "unique3", AppInfoId = 100 };

			Assert.ThrowsAsync(typeof(DbUpdateException), async () => await service.AddImage(addModel));
		}

		//Update
		[Test]
		public async Task UpdateImage_DbContainsUpdatedElem()
		{
			UpdateImageModel updateModel = new UpdateImageModel() { ImageName = "Image3", Description = "desc3", UniqueName = "unique3", AppInfoId = 1 };

			await service.UpdateImage(2, updateModel);
			using var context = new MainDbContext(options);

			Assert.That(context.Images.Any(elem => elem.ImageName == updateModel.ImageName && elem.Description == updateModel.Description
										&& (elem.AppInfo != null) && elem.UniqueName == updateModel.UniqueName), Is.True);
		}

		[Test]
		public void UpdateImage_ThrowsProcessExceptionNotFound()
		{
			UpdateImageModel updateModel = new UpdateImageModel() {ImageName = "Image3", Description = "desc3", UniqueName = "unique3", AppInfoId = 1 };

			Assert.ThrowsAsync(typeof(ProcessException), async () => await service.UpdateImage(100, updateModel));
		}

		[Test]
		public void UpdateImage_ThrowsValidationException()
		{
			UpdateImageModel updateModel = new UpdateImageModel() {ImageName = "Image3", Description = "desc3", UniqueName = "unique3", AppInfoId = 1 };
			updateValidatorMock.Setup(model => model.Check(It.IsAny<UpdateImageModel>())).Callback(() => throw new ValidationException("Error"));

			Assert.ThrowsAsync(typeof(ValidationException), async () => await service.UpdateImage(1, updateModel));
		}

		[Test]
		public void UpdateImage_ModelWithNonExistingForeighKey_ThrowsException()
		{
			UpdateImageModel updateModel = new UpdateImageModel() { ImageName = "Image3", Description = "desc3", UniqueName = "unique3", AppInfoId = 100 };

			Assert.ThrowsAsync(typeof(DbUpdateException), async () => await service.UpdateImage(1, updateModel));
		}

		//Delete
		[Test]
		public async Task DeleteImage_DbDoNotContainDeletedElem()
		{
			await service.DeleteImage(1);
			using var context = new MainDbContext(options);

			Assert.That(context.Images.Any(elem => elem.UniqueName == "unique1"), Is.False);
		}

		[Test]
		public void DeleteImage_ThrowsProcessExceptionNotFound()
		{
			Assert.ThrowsAsync(typeof(ProcessException), async () => await service.DeleteImage(100));
		}
	}
}
