namespace PromoWeb.Services.Questions
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionModel>> GetQuestions(int offset = 0, int limit = 10);
        Task<QuestionModel> GetQuestion(int questionId);
        Task<QuestionModel> AddQuestion(AddQuestionModel model);
        Task DeleteQuestion(int questionId);
    }
}
