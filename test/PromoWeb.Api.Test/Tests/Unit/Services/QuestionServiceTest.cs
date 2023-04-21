using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework.Internal;
using PromoWeb.Common.Exceptions;
using PromoWeb.Context;
using PromoWeb.Services.EmailSender;
using PromoWeb.Services.Questions;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	[TestFixture]
	public partial class QuestionServiceTest
	{
		//Get
		[Test]
		public async Task GetQuestions_ReturnsNotNull()
		{
			var models = await service.GetQuestions();

			Assert.NotNull(models);
		}

		[Test]
		public async Task GetQuestions_ReturnsExpectedType()
		{
			var models = await service.GetQuestions();

			Assert.IsInstanceOf(typeof(IEnumerable<QuestionModel>), models);
		}

		[Test]
		public async Task GetQuestions_ReturnsExpectedValues()
		{
			var models = await service.GetQuestions();

			Assert.That(models.Count(), Is.EqualTo(2));
			Assert.That(models.ElementAt(0).Text, Is.EqualTo("Question1"));
			Assert.That(models.ElementAt(1).Email, Is.EqualTo("mail2@tst.com"));
			Assert.That(models.ElementAt(0).Answer, Is.EqualTo("Answer1"));
		}

		[Test]
		public async Task GetQuestionsWithOffset_ReturnsOneElement()
		{
			var models = await service.GetQuestions(1);

			Assert.That(models.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task GetQuestionsWithInvalidOffset_ReturnsTwoElements()
		{
			var models = await service.GetQuestions(-100);

			Assert.That(models.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task GetQuestionsWithOffsetLimit_ReturnsEmptyCollection()
		{
			var models = await service.GetQuestions(10000000, 10000000);

			Assert.That(models.Count, Is.EqualTo(0));
		}

		//GetById
		[Test]
		public async Task GetQuestionById_ReturnsNotNull()
		{
			var model = await service.GetQuestion(1);

			Assert.NotNull(model);
		}

		[Test]
		public async Task GetQuestionByIdWithNonExistingId_ReturnsNull()
		{
			var model = await service.GetQuestion(500);

			Assert.IsNull(model);
		}

		[Test]
		public async Task GetQuestionById_ReturnsExpectedType()
		{
			var model = await service.GetQuestion(1);

			Assert.IsInstanceOf(typeof(QuestionModel), model);
		}

		[Test]
		public async Task GetQuestionById_ReturnsExpectedValue()
		{
			var model = await service.GetQuestion(2);

			Assert.That(model.Id, Is.EqualTo(2));
			Assert.That(model.Text, Is.EqualTo("Question2"));
			Assert.That(model.Answer, Is.Empty);
			Assert.That(model.Email, Is.EqualTo("mail2@tst.com"));
			Assert.That(model.Date, Is.EqualTo(new DateTime(2022, 12, 12)));
		}

		//Add
		[Test]
		public async Task AddQuestion_ReturnsNotNull()
		{
			AddQuestionModel addModel = new AddQuestionModel() { Text = "Question3", Date = new DateTime(2023, 2, 12), Email = "mail3@tst.com" };

			var model = await service.AddQuestion(addModel);

			Assert.That(model, Is.Not.Null);
		}

		[Test]
		public async Task AddQuestion_ReturnsExpectedType()
		{
			AddQuestionModel addModel = new AddQuestionModel() { Text = "Question3", Date = new DateTime(2023, 2, 12), Email = "mail3@tst.com" };

			var model = await service.AddQuestion(addModel);

			Assert.That(model, Is.TypeOf(typeof(QuestionModel)));
		}

		[Test]
		public async Task AddQuestion_ReturnsExpectedValue()
		{
			AddQuestionModel addModel = new AddQuestionModel() { Text = "Question3", Date = new DateTime(2023, 2, 12), Email = "mail3@tst.com" };

			var model = await service.AddQuestion(addModel);

			Assert.That(model.Text, Is.EqualTo(addModel.Text));
			Assert.That(model.Date, Is.EqualTo(addModel.Date));
			Assert.That(model.Answer, Is.Empty);
			Assert.That(model.Email, Is.EqualTo(addModel.Email));
		}

		[Test]
		public async Task AddQuestion_DbContainsAddedElem()
		{
			AddQuestionModel addModel = new AddQuestionModel() { Text = "Question3", Date = new DateTime(2023, 2, 12), Email = "mail3@tst.com" };

			await service.AddQuestion(addModel);
			using var context = new MainDbContext(options);

			Assert.That(context.Questions.Any(elem => elem.Text == addModel.Text && elem.Date == addModel.Date), Is.True);
		}

		[Test]
		public void AddQuestion_ThrowsValidationException()
		{
			AddQuestionModel addModel = new AddQuestionModel() { };

			addValidatorMock.Setup(model => model.Check(It.IsAny<AddQuestionModel>())).Callback(() => throw new ValidationException("Error"));

			Assert.ThrowsAsync(typeof(ValidationException), async () => await service.AddQuestion(addModel));
		}

		[Test]
		public async Task AddQuestion_SendEmailCalled()
		{
			AddQuestionModel addModel = new AddQuestionModel() { Text = "Question3", Date = new DateTime(2023, 2, 12), Email = "mail3@tst.com" };

			await service.AddQuestion(addModel);

			actionsMock.Verify(x => x.SendEmail(It.IsAny<EmailModel>()), Times.Once);
		}

		[Test]
		public async Task AddQuestion_SendEmailNotCalled()
		{
			AddQuestionModel addModel = new AddQuestionModel() { Text = "Question3", Date = new DateTime(2023, 2, 12) };

			await service.AddQuestion(addModel);

			actionsMock.Verify(x => x.SendEmail(It.IsAny<EmailModel>()), Times.Never);
		}

		//Delete
		[Test]
		public async Task DeleteQuestion_DbDoNotContainDeletedElem()
		{
			await service.DeleteQuestion(1);
			using var context = new MainDbContext(options);

			Assert.That(context.Questions.Any(elem => elem.Id == 1), Is.False);
		}

		[Test]
		public async Task DeleteQuestion_DeletesRelatedAnswer()
		{
			await service.DeleteQuestion(1);
			using var context = new MainDbContext(options);

			Assert.That(context.Answers.Any(elem => elem.QuestionId == 1), Is.False);
		}

		[Test]
		public void DeleteQuestion_ThrowsProcessExceptionNotFound()
		{
			Assert.ThrowsAsync(typeof(ProcessException), async () => await service.DeleteQuestion(100));
		}
	}
}
