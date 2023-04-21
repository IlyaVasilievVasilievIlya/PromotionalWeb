using FluentValidation;
using Moq;
using PromoWeb.Common.Exceptions;
using PromoWeb.Context;
using PromoWeb.Services.Sections;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	[TestFixture]
	public partial class SectionServiceTest
	{
		//Get
		[Test]
		public async Task GetSections_ReturnsNotNull()
		{
			var models = await service.GetSections();

			Assert.NotNull(models);
		}

		[Test]
		public async Task GetSections_ReturnsExpectedType()
		{
			var models = await service.GetSections();

			Assert.IsInstanceOf(typeof(IEnumerable<SectionModel>), models);
		}

		[Test]
		public async Task GetSections_ReturnsExpectedValues()
		{
			var models = await service.GetSections();

			Assert.That(models.Count(), Is.EqualTo(2));
			Assert.That(models.ElementAt(0).Id, Is.EqualTo(1));
			Assert.That(models.ElementAt(1).SectionName, Is.EqualTo("Section2"));
		}

		[Test]
		public async Task GetSectionsWithOffset_ReturnsOneElement()
		{
			var models = await service.GetSections(1);

			Assert.That(models.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task GetSectionsWithInvalidOffset_ReturnsTwoElements()
		{
			var models = await service.GetSections(-100);

			Assert.That(models.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task GetSectionsWithOffsetLimit_ReturnsEmptyCollection()
		{
			var models = await service.GetSections(10000000, 10000000);

			Assert.That(models.Count, Is.EqualTo(0));
		}


		//GetById
		[Test]
		public async Task GetSectionById_ReturnsNotNull()
		{
			var model = await service.GetSection(1);

			Assert.NotNull(model);
		}

		[Test]
		public async Task GetSectionByIdWithNonExistingId_ReturnsNull()
		{
			var model = await service.GetSection(500);

			Assert.IsNull(model);
		}

		[Test]
		public async Task GetSectionById_ReturnsExpectedType()
		{
			var model = await service.GetSection(1);

			Assert.IsInstanceOf(typeof(SectionModel), model);
		}

		[Test]
		public async Task GetSectionById_ReturnsExpectedValue()
		{
			var model = await service.GetSection(1);

			Assert.That(model.Id, Is.EqualTo(1));
			Assert.That(model.SectionName, Is.EqualTo("Section1"));
		}

		//Add
		[Test]
		public async Task AddSection_ReturnsNotNull()
		{
			AddSectionModel addModel = new AddSectionModel() { SectionName = "Section3" };

			var model = await service.AddSection(addModel);

			Assert.That(model, Is.Not.Null);
		}

		[Test]
		public async Task AddSection_ReturnsExpectedType()
		{
			AddSectionModel addModel = new AddSectionModel() { SectionName = "Section3" };

			var model = await service.AddSection(addModel);

			Assert.That(model, Is.TypeOf(typeof(SectionModel)));
		}

		[Test]
		public async Task AddSection_ReturnsExpectedValue()
		{
			AddSectionModel addModel = new AddSectionModel() { SectionName = "Section3" };

			var model = await service.AddSection(addModel);

			Assert.That(model.SectionName, Is.EqualTo(addModel.SectionName));
		}

		[Test]
		public async Task AddSection_DbContainsAddedElem()
		{
			AddSectionModel addModel = new AddSectionModel() { SectionName = "Section3" };

			await service.AddSection(addModel);
			using var context = new MainDbContext(options);

			Assert.That(context.Sections.Any(elem => elem.SectionName == addModel.SectionName), Is.True);
		}

		[Test]
		public void AddSection_ThrowsValidationException()
		{
			AddSectionModel addModel = new AddSectionModel() { SectionName = "Section3" };
			addValidatorMock.Setup(model => model.Check(It.IsAny<AddSectionModel>())).Callback(() => throw new ValidationException("Error"));

			Assert.ThrowsAsync(typeof(ValidationException), async () => await service.AddSection(addModel));
		}


		//Update
		[Test]
		public async Task UpdateSection_DbContainsUpdatedElem()
		{
			UpdateSectionModel updateModel = new UpdateSectionModel() { SectionName = "Section3" };

			await service.UpdateSection(2, updateModel);
			using var context = new MainDbContext(options);

			Assert.That(context.Sections.Any(elem => elem.SectionName == updateModel.SectionName), Is.True);
		}

		[Test]
		public void UpdateSection_ThrowsProcessExceptionNotFound()
		{
			UpdateSectionModel updateModel = new UpdateSectionModel() { SectionName = "Section3" };

			Assert.ThrowsAsync(typeof(ProcessException), async () => await service.UpdateSection(100, updateModel));
		}

		[Test]
		public void UpdateSection_ThrowsValidationException()
		{
			UpdateSectionModel updateModel = new UpdateSectionModel() { SectionName = "Section3" };
			updateValidatorMock.Setup(model => model.Check(It.IsAny<UpdateSectionModel>())).Callback(() => throw new ValidationException("Error"));

			Assert.ThrowsAsync(typeof(ValidationException), async () => await service.UpdateSection(1, updateModel));
		}


		//Delete
		[Test]
		public async Task DeleteSection_DbDoNotContainDeletedElem()
		{
			await service.DeleteSection(1);
			using var context = new MainDbContext(options);

			Assert.That(context.Sections.Any(elem => elem.Id == 1), Is.False);
		}

		[Test]
		public void DeleteSection_ThrowsProcessExceptionNotFound()
		{
			Assert.ThrowsAsync(typeof(ProcessException), async () => await service.DeleteSection(100));
		}
	}
}
