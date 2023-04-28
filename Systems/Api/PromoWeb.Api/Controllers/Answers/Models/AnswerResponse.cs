namespace PromoWeb.Services.Answers;

using AutoMapper;
using PromoWeb.Context.Entities;

public class AnswerResponse
{
	/// <summary>
	/// Answer id
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Question id
	/// </summary>
	public int QuestionId { get; set; }

	/// <summary>
	/// Question text
	/// </summary>
	public string Question { get; set; } = string.Empty;

	/// <summary>
	/// Answer text
	/// </summary>
	public string Text { get; set; } = string.Empty;

	/// <summary>
	/// Answer date
	/// </summary>
	public DateTime Date { get; set; }
}

public class AnswerResponseProfile : Profile
{
    public AnswerResponseProfile()
    {
        CreateMap<AnswerModel, AnswerResponse>();
    }
}
