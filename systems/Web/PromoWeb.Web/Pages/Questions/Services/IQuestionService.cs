namespace PromoWeb.Web
{
	public interface IQuestionService
	{
		Task<IEnumerable<QuestionListItem>> GetQuestions(int offset = 0, int limit = 10);
		Task<QuestionListItem> GetQuestion(int questionId);
		Task AddQuestion(QuestionModel request);
		Task DeleteQuestion(int questionId);
	}
}
