namespace PromoWeb.Services.Answers
{
    public interface IAnswerService
    {
        Task<IEnumerable<AnswerModel>> GetAnswers(int offset = 0, int limit = 10);
        Task<AnswerModel> GetAnswer(int answerId);
        Task<AnswerModel> AddAnswer(AddAnswerModel model);
        Task DeleteAnswer(int answerId);
    }
}
