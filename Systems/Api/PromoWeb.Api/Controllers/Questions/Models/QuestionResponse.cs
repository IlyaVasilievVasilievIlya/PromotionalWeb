namespace PromoWeb.Api.Controllers;

using AutoMapper;
using PromoWeb.Services.Questions;

public class QuestionResponse
{
	/// <summary>
	/// Question Id
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Creation date
	/// </summary>
	public DateTime Date { get; set; }

	/// <summary>
	/// Question text
	/// </summary>
	public string Text { get; set; } = string.Empty;

	/// <summary>
	/// Email for answer
	/// </summary>
	public string Email { get; set; } = string.Empty;

	/// <summary>
	/// Answer
	/// </summary>
	public string Answer { get; set; } = string.Empty;
}

public class QuestionResponseProfile : Profile
{
    public QuestionResponseProfile()
    {
        CreateMap<QuestionModel, QuestionResponse>();
    }
}
