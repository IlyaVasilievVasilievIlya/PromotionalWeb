using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using PromoWeb.Common.Validator;
using PromoWeb.Context.Entities;
using PromoWeb.Context;
using PromoWeb.Services.Actions;
using PromoWeb.Services.Answers;
using PromoWeb.Services.EmailSender;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	public partial class AnswerServiceTest
	{
		SqliteConnection connection;
		Mock<IDbContextFactory<MainDbContext>> factoryMock;
		Mock<IMapper> mapperMock;
		Mock<IAction> actionsMock;
		Mock<IModelValidator<AddAnswerModel>> addValidatorMock;
		AnswerService service;
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
			mapperMock.Setup(mapper => mapper.Map<AnswerModel?>(It.IsAny<Answer>()))
				.Returns((Answer? answer) => (answer is null) ? null
					 : new AnswerModel
					 {
						 Id = answer!.Id,
						 Date = answer.Date,
						 Text = answer.Text,
						 Question = answer.Question.Text,
						 QuestionId = answer.QuestionId
					 });

			mapperMock.Setup(mapper => mapper.Map<Answer>(It.IsAny<AddAnswerModel>()))
				.Returns((AddAnswerModel model) =>
					new Answer
					{
						Date = model.Date,
						Text = model.Text,
						QuestionId = model.QuestionId
					});

			actionsMock = new Mock<IAction>();
			actionsMock.Setup(action => action.SendEmail(It.IsAny<EmailModel>()));

			addValidatorMock = new Mock<IModelValidator<AddAnswerModel>>();
			addValidatorMock.Setup(model => model.Check(It.IsAny<AddAnswerModel>()));
		}


		[SetUp]
		public void Setup()
		{
			using (var context = new MainDbContext(options))
			{
				context.Database.EnsureCreated();

				Question[] questions = new Question[]
				{
					new Question { Id = 1, Text = "Question1", Date = new DateTime(2022, 12, 12) },
					new Question { Id = 2, Text = "Question2", Email = "mail2@tst.com", Date = new DateTime(2022, 12, 28) },
					new Question { Id = 3, Text = "Question3", Email = "mail3@tst.com", Date = new DateTime(2022, 12, 30) },
					new Question { Id = 4, Text = "Question4", Date = new DateTime(2022, 4, 4) }
				};
				context.Questions.AddRange(questions);

				Answer[] answers = new Answer[2]
				{
					new Answer() { Id = 1, Question = questions[0], Text = "Answer1", Date = new DateTime(2023, 3, 11) },
					new Answer() { Id = 2, Question = questions[1], Text = "Answer2", Date = new DateTime(2023, 2, 12) }
				};
				context.Answers.AddRange(answers);

				context.SaveChanges();
			}

			service = new AnswerService(factoryMock.Object, mapperMock.Object, actionsMock.Object, addValidatorMock.Object);
		}

		[TearDown]
		public void ClearDb()
		{
			using var context = new MainDbContext(options);
			context.Answers.RemoveRange(context.Answers);
			context.Questions.RemoveRange(context.Questions);
			context.SaveChanges();
			actionsMock.Invocations.Clear();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			connection.Dispose();
		}
	}
}
