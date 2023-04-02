namespace PromoWeb.Web
{
	public interface IAnswerService
	{
		Task AddAnswer(AnswerModel request);
		Task DeleteAnswer(int answerId);
	}
}
