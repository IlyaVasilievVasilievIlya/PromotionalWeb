using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PromoWeb.Common.Exceptions;
using PromoWeb.Common.Validator;
using PromoWeb.Context;
using PromoWeb.Context.Entities;
using PromoWeb.Services.Actions;
using PromoWeb.Services.EmailSender;

namespace PromoWeb.Services.Answers
{
    public class AnswerService : IAnswerService
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;
        private readonly IMapper mapper;
        private readonly IAction actions;
        private readonly IModelValidator<AddAnswerModel> addAnswerModelValidator;

        public AnswerService(
            IDbContextFactory<MainDbContext> contextFactory,
            IMapper mapper,
            IAction actions,
            IModelValidator<AddAnswerModel> addAnswerModelValidator
            )
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
            this.addAnswerModelValidator = addAnswerModelValidator;
            this.actions = actions;
        }

        public async Task<IEnumerable<AnswerModel>> GetAnswers(int offset = 0, int limit = 10)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var answers = context
                .Answers
                .Include(x => x.Question)
                .AsQueryable();

            answers = answers
                .Skip(Math.Max(offset, 0))
                .Take(Math.Max(0, Math.Min(limit, 1000)));

            var data = (await answers.ToListAsync()).Select(answer => mapper.Map<AnswerModel>(answer));

            return data;
        }

        public async Task<AnswerModel> GetAnswer(int id)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var answer = await context.Answers.Include(x => x.Question).FirstOrDefaultAsync(x => x.Id.Equals(id));

            var data = mapper.Map<AnswerModel>(answer);

            return data;
        }

        public async Task<AnswerModel> AddAnswer(AddAnswerModel model)
        {
            addAnswerModelValidator.Check(model);

            using var context = await contextFactory.CreateDbContextAsync();

            var answer = mapper.Map<Answer>(model);
            await context.Answers.AddAsync(answer);
            context.SaveChanges();

            var question = await context.Questions.FirstOrDefaultAsync(x => x.Id.Equals(model.QuestionId));

            if (question.Email is not null)
                await actions.SendEmail(new EmailModel
                {
                    Email = question.Email,
                    Subject = "Your PromoWeb question",
                    Message = model.Text
                });


            return mapper.Map<AnswerModel>(answer);
        }

        public async Task DeleteAnswer(int answerId)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var answer = await context.Answers.FirstOrDefaultAsync(x => x.Id.Equals(answerId))
                ?? throw new ProcessException($"The answer (id: {answerId}) was not found");

            context.Remove(answer);
            context.SaveChanges();
        }
    }
}
