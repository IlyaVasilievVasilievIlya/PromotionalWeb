namespace PromoWeb.Web
{
	public interface IAnswerService
	{
		Task<IEnumerable<AnswerListItem>> GetAnswers(int offset = 0, int limit = 10);
		Task AddAnswer(AnswerModel request);
		Task DeleteAnswer(int answerId);
	}
}
