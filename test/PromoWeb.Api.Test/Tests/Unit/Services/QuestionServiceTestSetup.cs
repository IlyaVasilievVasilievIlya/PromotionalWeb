using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using PromoWeb.Common.Validator;
using PromoWeb.Context.Entities;
using PromoWeb.Context;
using PromoWeb.Services.Questions;
using PromoWeb.Services.Actions;
using PromoWeb.Services.EmailSender;

namespace PromoWeb.Api.Test.Tests.Unit.Services
{
	public partial class QuestionServiceTest
	{
		SqliteConnection connection;
		Mock<IDbContextFactory<MainDbContext>> factoryMock;
		Mock<IMapper> mapperMock;
		Mock<IAction> actionsMock;
		Mock<IModelValidator<AddQuestionModel>> addValidatorMock;
		QuestionService service;
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
			mapperMock.Setup(mapper => mapper.Map<QuestionModel?>(It.IsAny<Question>()))
				.Returns((Question? question) => (question is null) ? null
					 : new QuestionModel
					 {
						 Id = question!.Id,
						 Date = question.Date,
						 Text = question.Text,
						 Email = question.Email ?? string.Empty,
						 Answer = question.Answer?.Text ?? string.Empty
					 });

			mapperMock.Setup(mapper => mapper.Map<Question>(It.IsAny<AddQuestionModel>()))
				.Returns((AddQuestionModel model) =>
					new Question
					{
						Date = model.Date,
						Text = model.Text,
						Email = model.Email
					});

			actionsMock = new Mock<IAction>();
			actionsMock.Setup(action => action.SendEmail(It.IsAny<EmailModel>()));

			addValidatorMock = new Mock<IModelValidator<AddQuestionModel>>();
			addValidatorMock.Setup(model => model.Check(It.IsAny<AddQuestionModel>()));
		}


		[SetUp]
		public void Setup()
		{
			using (var context = new MainDbContext(options))
			{
				context.Database.EnsureCreated();
				Question[] questions = new Question[2] 
				{ 
					new Question { Id = 1, Text = "Question1" },
					new Question { Id = 2, Text = "Question2", Email = "mail2@tst.com", Date = new DateTime(2022, 12, 12) }
				};
				context.Questions.AddRange(questions);

				context.Answers.Add(new Answer() { Id = 1, Question = questions[0], Text = "Answer1" });


				context.SaveChanges();
			}
			service = new QuestionService(factoryMock.Object, mapperMock.Object, addValidatorMock.Object, actionsMock.Object);
		}

		[TearDown]
		public void ClearDb()
		{
			using var context = new MainDbContext(options);
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
