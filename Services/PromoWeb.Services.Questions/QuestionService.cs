using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PromoWeb.Common.Exceptions;
using PromoWeb.Common.Validator;
using PromoWeb.Context;
using PromoWeb.Context.Entities;

namespace PromoWeb.Services.Questions
{
    public class QuestionService : IQuestionService
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;
        private readonly IMapper mapper;
        private readonly IModelValidator<AddQuestionModel> addQuestionModelValidator;

        public QuestionService(
            IDbContextFactory<MainDbContext> contextFactory,
            IMapper mapper,
            IModelValidator<AddQuestionModel> addQuestionModelValidator
            )
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
            this.addQuestionModelValidator = addQuestionModelValidator;
        }

        public async Task<IEnumerable<QuestionModel>> GetQuestions(int offset = 0, int limit = 10)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var questions = context
                .Questions
                .Include(x => x.Answer)
                .AsQueryable();

            questions = questions
                .Skip(Math.Max(offset, 0))
                .Take(Math.Max(0, Math.Min(limit, 1000)));

            var data = (await questions.ToListAsync()).Select(question => mapper.Map<QuestionModel>(question));

            return data;
        }

        public async Task<QuestionModel> GetQuestion(int id)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var answer = await context.Questions.Include(x => x.Answer).FirstOrDefaultAsync(x => x.Id.Equals(id));

            var data = mapper.Map<QuestionModel>(answer);

            return data;
        }

        public async Task<QuestionModel> AddQuestion(AddQuestionModel model)
        {
            addQuestionModelValidator.Check(model);

            using var context = await contextFactory.CreateDbContextAsync();

            var question = mapper.Map<Question>(model);
            await context.Questions.AddAsync(question);
            context.SaveChanges();

            return mapper.Map<QuestionModel>(question);
        }

        public async Task DeleteQuestion(int questionId)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var question = await context.Questions.FirstOrDefaultAsync(x => x.Id.Equals(questionId))
                ?? throw new ProcessException($"The question (id: {questionId}) was not found");

            context.Remove(question);
            context.SaveChanges();
        }
    }
}
