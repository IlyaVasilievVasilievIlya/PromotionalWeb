using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using PromoWeb.Common.Exceptions;
using PromoWeb.Context;
using PromoWeb.Services.Links;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	[TestFixture]
	public partial class LinkServiceTest
	{
		//Get
		[Test]
		public async Task GetLinks_ReturnsNotNull()
		{
			var models = await service.GetLinks();

			Assert.NotNull(models);
		}

		[Test]
		public async Task GetLinks_ReturnsExpectedType()
		{
			var models = await service.GetLinks();

			Assert.IsInstanceOf(typeof(IEnumerable<LinkModel>), models);
		}

		[Test]
		public async Task GetLinks_ReturnsExpectedValues()
		{
			var models = await service.GetLinks();

			Assert.That(models.Count(), Is.EqualTo(2));
			Assert.That(models.ElementAt(0).LinkText, Is.EqualTo("Link1"));
			Assert.That(models.ElementAt(1).Id, Is.EqualTo(2));
			Assert.That(models.ElementAt(0).Section, Is.EqualTo("Section1"));
		}

		[Test]
		public async Task GetLinksWithOffset_ReturnsOneElement()
		{
			var models = await service.GetLinks(1);

			Assert.That(models.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task GetLinksWithInvalidOffset_ReturnsTwoElements()
		{
			var models = await service.GetLinks(-100);

			Assert.That(models.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task GetLinksWithOffsetLimit_ReturnsEmptyCollection()
		{
			var models = await service.GetLinks(10000000, 10000000);

			Assert.That(models.Count, Is.EqualTo(0));
		}

		//GetBySectionId
		[Test]
		public async Task GetLinksBySectionId_ReturnsNotNull()
		{
			var models = await service.GetLinksBySectionId(1);

			Assert.NotNull(models);
		}

		[Test]
		public async Task GetLinksBySectionId_ReturnsExpectedType()
		{
			var models = await service.GetLinksBySectionId(1);

			Assert.IsInstanceOf(typeof(IEnumerable<LinkModel>), models);
		}

		[Test]
		public async Task GetLinksBySectionId_ReturnsValuesWithSameAppInfo()
		{
			var models = await service.GetLinksBySectionId(1);

			Assert.That(models.Count(), Is.EqualTo(2));
			Assert.That(models.ElementAt(0).LinkText, Is.EqualTo("Link1"));
			Assert.That(models.ElementAt(0).Id, Is.EqualTo(1));
			Assert.That(models, Has.All.With.Property("SectionId").EqualTo(1));
		}

		//GetById
		[Test]
		public async Task GetLinkById_ReturnsNotNull()
		{
			var model = await service.GetLink(1);

			Assert.NotNull(model);
		}

		[Test]
		public async Task GetLinkByIdWithNonExistingId_ReturnsNull()
		{
			var model = await service.GetLink(500);

			Assert.IsNull(model);
		}

		[Test]
		public async Task GetLinkById_ReturnsExpectedType()
		{
			var model = await service.GetLink(1);

			Assert.IsInstanceOf(typeof(LinkModel), model);
		}

		[Test]
		public async Task GetLinkById_ReturnsExpectedValue()
		{
			var model = await service.GetLink(1);

			Assert.That(model.Id, Is.EqualTo(1));
			Assert.That(model.LinkText, Is.EqualTo("Link1"));
			Assert.That(model.Description, Is.EqualTo("desc1"));
			Assert.That(model.SectionId, Is.EqualTo(1));
			Assert.That(model.Section, Is.EqualTo("Section1"));
		}

		//Add
		[Test]
		public async Task AddLink_ReturnsNotNull()
		{
			AddLinkModel addModel = new AddLinkModel() { LinkText = "Link3", Description = "desc3", SectionId = 1 };

			var model = await service.AddLink(addModel);

			Assert.That(model, Is.Not.Null);
		}

		[Test]
		public async Task AddLink_ReturnsExpectedType()
		{
			AddLinkModel addModel = new AddLinkModel() { LinkText = "Link3", Description = "desc3", SectionId = 1 };

			var model = await service.AddLink(addModel);

			Assert.That(model, Is.TypeOf(typeof(LinkModel)));
		}

		[Test]
		public async Task AddLink_ReturnsExpectedValue()
		{
			AddLinkModel addModel = new AddLinkModel() {LinkText = "Link3", Description = "desc3", SectionId = 1 };

			var model = await service.AddLink(addModel);

			Assert.That(model.LinkText, Is.EqualTo(addModel.LinkText));
			Assert.That(model.Description, Is.EqualTo(addModel.Description));
			Assert.That(model.SectionId, Is.EqualTo(addModel.SectionId));

		}

		[Test]
		public async Task AddLink_DbContainsAddedElem()
		{
			AddLinkModel addModel = new AddLinkModel() {LinkText = "Link3", Description = "desc3", SectionId = 1 };

			await service.AddLink(addModel);
			using var context = new MainDbContext(options);

			Assert.That(context.Links.Any(elem => elem.LinkText == addModel.LinkText && elem.Description == addModel.Description
													&& (elem.Section != null) && elem.SectionId == addModel.SectionId), Is.True);
		}

		[Test]
		public void AddLink_ThrowsValidationException()
		{
			AddLinkModel addModel = new AddLinkModel() {LinkText = "Link3", Description = "desc3", SectionId = 1 };

			addValidatorMock.Setup(model => model.Check(It.IsAny<AddLinkModel>())).Callback(() => throw new ValidationException("Error"));

			Assert.ThrowsAsync(typeof(ValidationException), async () => await service.AddLink(addModel));
		}

		[Test]
		public void AddLink_ModelWithNonExistingForeighKey_ThrowsException()
		{
			AddLinkModel addModel = new AddLinkModel() { LinkText = "Link3", Description = "desc3", SectionId = 100 };

			Assert.ThrowsAsync(typeof(DbUpdateException), async () => await service.AddLink(addModel));
		}

		//Update
		[Test]
		public async Task UpdateLink_DbContainsUpdatedElem()
		{
			UpdateLinkModel updateModel = new UpdateLinkModel() { LinkText = "Link3", Description = "desc3", SectionId = 1 };

			await service.UpdateLink(2, updateModel);
			using var context = new MainDbContext(options);

			Assert.That(context.Links.Any(elem => elem.LinkText == updateModel.LinkText && elem.Description == updateModel.Description
													&& (elem.Section != null) && elem.SectionId == updateModel.SectionId), Is.True);
		}

		[Test]
		public void UpdateLink_ThrowsProcessExceptionNotFound()
		{
			UpdateLinkModel updateModel = new UpdateLinkModel() { LinkText = "Link3", Description = "desc3", SectionId = 1 };

			Assert.ThrowsAsync(typeof(ProcessException), async () => await service.UpdateLink(100, updateModel));
		}

		[Test]
		public void UpdateLink_ThrowsValidationException()
		{
			UpdateLinkModel updateModel = new UpdateLinkModel() { LinkText = "Link3", Description = "desc3", SectionId = 1 };
			updateValidatorMock.Setup(model => model.Check(It.IsAny<UpdateLinkModel>())).Callback(() => throw new ValidationException("Error"));

			Assert.ThrowsAsync(typeof(ValidationException), async () => await service.UpdateLink(1, updateModel));
		}

		[Test]
		public void UpdateLink_ModelWithNonExistingForeighKey_ThrowsException()
		{
			UpdateLinkModel updateModel = new UpdateLinkModel() { LinkText = "Link3", Description = "desc3", SectionId = 100 };

			Assert.ThrowsAsync(typeof(DbUpdateException), async () => await service.UpdateLink(1, updateModel));
		}

		//Delete
		[Test]
		public async Task DeleteLink_DbDoNotContainDeletedElem()
		{
			await service.DeleteLink(1);
			using var context = new MainDbContext(options);

			Assert.That(context.Links.Any(elem => elem.Id == 1), Is.False);
		}

		[Test]
		public void DeleteLink_ThrowsProcessExceptionNotFound()
		{
			Assert.ThrowsAsync(typeof(ProcessException), async () => await service.DeleteLink(100));
		}
	}
}
