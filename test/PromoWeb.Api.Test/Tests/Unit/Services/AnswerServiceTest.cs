using Moq;
using PromoWeb.Common.Exceptions;
using PromoWeb.Context;
using PromoWeb.Services.EmailSender;
using PromoWeb.Services.Answers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PromoWeb.Services.Links;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	[TestFixture]
	public partial class AnswerServiceTest
	{
		//Get
		[Test]
		public async Task GetAnswers_ReturnsNotNull()
		{
			var models = await service.GetAnswers();

			Assert.NotNull(models);
		}

		[Test]
		public async Task GetAnswers_ReturnsExpectedType()
		{
			var models = await service.GetAnswers();

			Assert.IsInstanceOf(typeof(IEnumerable<AnswerModel>), models);
		}

		[Test]
		public async Task GetAnswers_ReturnsExpectedValues()
		{
			var models = await service.GetAnswers();

			Assert.That(models.Count(), Is.EqualTo(2));
			Assert.That(models.ElementAt(0).Text, Is.EqualTo("Answer1"));
			Assert.That(models.ElementAt(1).QuestionId, Is.EqualTo(2));
			Assert.That(models.ElementAt(0).Date, Is.EqualTo(new DateTime(2023, 3, 11)));
		}

		[Test]
		public async Task GetAnswersWithOffset_ReturnsOneElement()
		{
			var models = await service.GetAnswers(1);

			Assert.That(models.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task GetAnswersWithInvalidOffset_ReturnsTwoElements()
		{
			var models = await service.GetAnswers(-100);

			Assert.That(models.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task GetAnswersWithOffsetLimit_ReturnsEmptyCollection()
		{
			var models = await service.GetAnswers(10000000, 10000000);

			Assert.That(models.Count, Is.EqualTo(0));
		}

		//GetById
		[Test]
		public async Task GetAnswerById_ReturnsNotNull()
		{
			var model = await service.GetAnswer(1);

			Assert.NotNull(model);
		}

		[Test]
		public async Task GetAnswerByIdWithNonExistingId_ReturnsNull()
		{
			var model = await service.GetAnswer(500);

			Assert.IsNull(model);
		}

		[Test]
		public async Task GetAnswerById_ReturnsExpectedType()
		{
			var model = await service.GetAnswer(1);

			Assert.IsInstanceOf(typeof(AnswerModel), model);
		}

		[Test]
		public async Task GetAnswerById_ReturnsExpectedValue()
		{
			var model = await service.GetAnswer(2);

			Assert.That(model.Id, Is.EqualTo(2));
			Assert.That(model.Text, Is.EqualTo("Answer2"));
			Assert.That(model.Question, Is.EqualTo("Question2"));
			Assert.That(model.QuestionId, Is.EqualTo(2));
			Assert.That(model.Date, Is.EqualTo(new DateTime(2023, 2, 12)));
		}

		//Add
		[Test]
		public async Task AddAnswer_ReturnsNotNull()
		{
			AddAnswerModel addModel = new AddAnswerModel() { Text = "Answer3", Date = new DateTime(2023, 1, 11), QuestionId = 3 };

			var model = await service.AddAnswer(addModel);

			Assert.That(model, Is.Not.Null);
		}

		[Test]
		public async Task AddAnswer_ReturnsExpectedType()
		{
			AddAnswerModel addModel = new AddAnswerModel() { Text = "Answer3", Date = new DateTime(2023, 1, 11), QuestionId = 3 };

			var model = await service.AddAnswer(addModel);

			Assert.That(model, Is.TypeOf(typeof(AnswerModel)));
		}

		[Test]
		public async Task AddAnswer_ReturnsExpectedValue()
		{
			AddAnswerModel addModel = new AddAnswerModel() { Text = "Answer3", Date = new DateTime(2023, 1, 11), QuestionId = 3 };

			var model = await service.AddAnswer(addModel);

			Assert.That(model.Text, Is.EqualTo(addModel.Text));
			Assert.That(model.QuestionId, Is.EqualTo(addModel.QuestionId));
			Assert.That(model.Date, Is.EqualTo(addModel.Date));
		}

		[Test]
		public async Task AddAnswer_DbContainsAddedElem()
		{
			AddAnswerModel addModel = new AddAnswerModel() { Text = "Answer3", Date = new DateTime(2023, 1, 11), QuestionId = 3 };

			await service.AddAnswer(addModel);
			using var context = new MainDbContext(options);
			bool isAdded = context.Answers.Any(elem => elem.Text == addModel.Text && elem.Date == addModel.Date 
												&& elem.QuestionId == addModel.QuestionId && elem.Question != null);

			Assert.That(isAdded, Is.True);
		}

		[Test]
		public void AddAnswer_ThrowsValidationException()
		{
			AddAnswerModel addModel = new AddAnswerModel() { Text = "Answer3", Date = new DateTime(2023, 1, 11), QuestionId = 3 };

			addValidatorMock.Setup(model => model.Check(It.IsAny<AddAnswerModel>())).Callback(() => throw new ValidationException("Error"));

			Assert.ThrowsAsync(typeof(ValidationException), async () => await service.AddAnswer(addModel));
		}

		[Test]
		public void AddAnswer_AnswerAlreadyExists_ThrowsException()
		{
			AddAnswerModel addModel = new AddAnswerModel() { Text = "Answer3", Date = new DateTime(2023, 1, 11), QuestionId = 2 };

			Assert.ThrowsAsync(typeof(DbUpdateException), async () => await service.AddAnswer(addModel));
		}

		[Test]
		public void AddAnswer_ModelWithNonExistingForeighKey_ThrowsException()
		{
			AddAnswerModel addModel = new AddAnswerModel() { Text = "Answer3", Date = new DateTime(2023, 1, 11), QuestionId = 100 };

			Assert.ThrowsAsync(typeof(DbUpdateException), async () => await service.AddAnswer(addModel));
		}

		[Test]
		public async Task AddAnswer_SendEmailCalled()
		{
			AddAnswerModel addModel = new AddAnswerModel() { Text = "Answer3", Date = new DateTime(2023, 1, 11), QuestionId = 3 };

			await service.AddAnswer(addModel);

			actionsMock.Verify(x => x.SendEmail(It.IsAny<EmailModel>()), Times.Once);
		}

		[Test]
		public async Task AddAnswer_SendEmailNotCalled()
		{
			AddAnswerModel addModel = new AddAnswerModel() { Text = "Answer3", Date = new DateTime(2023, 1, 11), QuestionId = 4 };

			await service.AddAnswer(addModel);

			actionsMock.Verify(x => x.SendEmail(It.IsAny<EmailModel>()), Times.Never);
		}

		//Delete
		[Test]
		public async Task DeleteAnswer_DbDoNotContainDeletedElem()
		{
			await service.DeleteAnswer(1);
			using var context = new MainDbContext(options);

			Assert.That(context.Answers.Any(elem => elem.Id == 1), Is.False);
		}

		[Test]
		public void DeleteAnswer_ThrowsProcessExceptionNotFound()
		{
			Assert.ThrowsAsync(typeof(ProcessException), async () => await service.DeleteAnswer(100));
		}
	}
}
