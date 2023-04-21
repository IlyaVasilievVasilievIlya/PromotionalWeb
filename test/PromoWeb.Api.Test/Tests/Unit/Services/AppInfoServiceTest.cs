using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using PromoWeb.Common.Exceptions;
using PromoWeb.Context;
using PromoWeb.Services.AppInfos;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	[TestFixture]
	public partial class AppInfoServiceTest
	{
		//Get
		[Test]
		public async Task GetAppInfos_ReturnsNotNull()
		{
			var models = await service.GetAppInfos();

			Assert.NotNull(models);
		}

		[Test]
		public async Task GetAppInfos_ReturnsExpectedType()
		{
			var models = await service.GetAppInfos();

			Assert.IsInstanceOf(typeof(IEnumerable<AppInfoModel>), models);
		}

		[Test]
		public async Task GetAppInfos_ReturnsExpectedValues()
		{
			var models = await service.GetAppInfos();

			Assert.That(models.Count(), Is.EqualTo(2));
			Assert.That(models.ElementAt(0).TextTitle, Is.EqualTo("Title1"));
			Assert.That(models.ElementAt(1).Id, Is.EqualTo(2));
			Assert.That(models.ElementAt(0).Section, Is.EqualTo("Section1"));
		}

		[Test]
		public async Task GetAppInfosWithOffset_ReturnsOneElement()
		{
			var models = await service.GetAppInfos(1);

			Assert.That(models.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task GetAppInfosWithInvalidOffset_ReturnsTwoElements()
		{
			var models = await service.GetAppInfos(-100);

			Assert.That(models.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task GetAppInfosWithOffsetLimit_ReturnsEmptyCollection()
		{
			var models = await service.GetAppInfos(10000000, 10000000);

			Assert.That(models.Count, Is.EqualTo(0));
		}

		//GetBySectionId
		[Test]
		public async Task GetAppInfosBySectionId_ReturnsNotNull()
		{
			var models = await service.GetAppInfosBySectionId(1);

			Assert.NotNull(models);
		}

		[Test]
		public async Task GetAppInfosBySectionId_ReturnsExpectedType()
		{
			var models = await service.GetAppInfosBySectionId(1);

			Assert.IsInstanceOf(typeof(IEnumerable<AppInfoModel>), models);
		}

		[Test]
		public async Task GetAppInfosBySectionId_ReturnsValuesWithSameAppInfo()
		{
			var models = await service.GetAppInfosBySectionId(1);

			Assert.That(models.Count(), Is.EqualTo(2));
			Assert.That(models.ElementAt(0).TextTitle, Is.EqualTo("Title1"));
			Assert.That(models.ElementAt(1).Id, Is.EqualTo(2));
			Assert.That(models, Has.All.With.Property("SectionId").EqualTo(1));
		}

		//GetById
		[Test]
		public async Task GetAppInfoById_ReturnsNotNull()
		{
			var model = await service.GetAppInfo(1);

			Assert.NotNull(model);
		}

		[Test]
		public async Task GetAppInfoByIdWithNonExistingId_ReturnsNull()
		{
			var model = await service.GetAppInfo(500);

			Assert.IsNull(model);
		}

		[Test]
		public async Task GetAppInfoById_ReturnsExpectedType()
		{
			var model = await service.GetAppInfo(1);

			Assert.IsInstanceOf(typeof(AppInfoModel), model);
		}

		[Test]
		public async Task GetAppInfoById_ReturnsExpectedValue()
		{
			var model = await service.GetAppInfo(1);

			Assert.That(model.Id, Is.EqualTo(1));
			Assert.That(model.TextTitle, Is.EqualTo("Title1"));
			Assert.That(model.Text, Is.EqualTo("Text1"));
			Assert.That(model.SectionId, Is.EqualTo(1));
			Assert.That(model.Section, Is.EqualTo("Section1"));
		}

		//Add
		[Test]
		public async Task AddAppInfo_ReturnsNotNull()
		{
			AddAppInfoModel addModel = new AddAppInfoModel() { TextTitle = "AppInfo3", Text = "desc3", SectionId = 2 };

			var model = await service.AddAppInfo(addModel);

			Assert.That(model, Is.Not.Null);
		}

		[Test]
		public async Task AddAppInfo_ReturnsExpectedType()
		{
			AddAppInfoModel addModel = new AddAppInfoModel() { TextTitle = "AppInfo3", Text = "desc3", SectionId = 2 };

			var model = await service.AddAppInfo(addModel);

			Assert.That(model, Is.TypeOf(typeof(AppInfoModel)));
		}

		[Test]
		public async Task AddAppInfo_ReturnsExpectedValue()
		{
			AddAppInfoModel addModel = new AddAppInfoModel() { TextTitle = "AppInfo3", Text = "desc3", SectionId = 2 };

			var model = await service.AddAppInfo(addModel);

			Assert.That(model.TextTitle, Is.EqualTo(addModel.TextTitle));
			Assert.That(model.Text, Is.EqualTo(addModel.Text));
			Assert.That(model.SectionId, Is.EqualTo(addModel.SectionId));

		}

		[Test]
		public async Task AddAppInfo_DbContainsAddedElem()
		{
			AddAppInfoModel addModel = new AddAppInfoModel() {TextTitle = "AppInfo3", Text = "desc3", SectionId = 2 };

			await service.AddAppInfo(addModel);
			using var context = new MainDbContext(options);

			Assert.That(context.AppInfos.Any(elem => elem.TextTitle == addModel.TextTitle && elem.Text == addModel.Text
													&& (elem.Section != null) && elem.SectionId == addModel.SectionId), Is.True);
		}

		[Test]
		public void AddAppInfo_ThrowsValidationException()
		{
			AddAppInfoModel addModel = new AddAppInfoModel() {TextTitle = "AppInfo3", Text = "desc3", SectionId = 2 };

			addValidatorMock.Setup(model => model.Check(It.IsAny<AddAppInfoModel>())).Callback(() => throw new ValidationException("Error"));

			Assert.ThrowsAsync(typeof(ValidationException), async () => await service.AddAppInfo(addModel));
		}

		[Test]
		public void AddAppInfo_ModelWithNonExistingForeighKey_ThrowsException()
		{
			AddAppInfoModel addModel = new AddAppInfoModel() { TextTitle = "AppInfo3", Text = "desc3", SectionId = 100 };

			Assert.ThrowsAsync(typeof(DbUpdateException), async () => await service.AddAppInfo(addModel));
		}

		//Update
		[Test]
		public async Task UpdateAppInfo_DbContainsUpdatedElem()
		{
			UpdateAppInfoModel updateModel = new UpdateAppInfoModel() { TextTitle = "AppInfo3", Text = "desc3", SectionId = 1 };

			await service.UpdateAppInfo(2, updateModel);
			using var context = new MainDbContext(options);

			Assert.That(context.AppInfos.Any(elem => elem.TextTitle == updateModel.TextTitle && elem.Text == updateModel.Text
													&& (elem.Section != null) && elem.SectionId == updateModel.SectionId), Is.True);
		}

		[Test]
		public void UpdateAppInfo_ThrowsProcessExceptionNotFound()
		{
			UpdateAppInfoModel updateModel = new UpdateAppInfoModel() { TextTitle = "AppInfo3", Text = "desc3", SectionId = 1 };

			Assert.ThrowsAsync(typeof(ProcessException), async () => await service.UpdateAppInfo(100, updateModel));
		}

		[Test]
		public void UpdateAppInfo_ThrowsValidationException()
		{
			UpdateAppInfoModel updateModel = new UpdateAppInfoModel() { TextTitle = "AppInfo3", Text = "desc3", SectionId = 1 };
			updateValidatorMock.Setup(model => model.Check(It.IsAny<UpdateAppInfoModel>())).Callback(() => throw new ValidationException("Error"));

			Assert.ThrowsAsync(typeof(ValidationException), async () => await service.UpdateAppInfo(1, updateModel));
		}

		[Test]
		public void UpdateAppInfo_ModelWithNonExistingForeighKey_ThrowsException()
		{
			UpdateAppInfoModel updateModel = new UpdateAppInfoModel() { TextTitle = "AppInfo3", Text = "desc3", SectionId = 100 };

			Assert.ThrowsAsync(typeof(DbUpdateException), async () => await service.UpdateAppInfo(1, updateModel));
		}

		//Delete
		[Test]
		public async Task DeleteAppInfo_DbDoNotContainDeletedElem()
		{
			await service.DeleteAppInfo(1);
			using var context = new MainDbContext(options);

			Assert.That(context.AppInfos.Any(elem => elem.Id == 1), Is.False);
		}

		[Test]
		public void DeleteAppInfo_ThrowsProcessExceptionNotFound()
		{
			Assert.ThrowsAsync(typeof(ProcessException), async () => await service.DeleteAppInfo(100));
		}
	}
}
