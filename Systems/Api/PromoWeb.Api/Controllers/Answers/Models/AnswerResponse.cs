namespace PromoWeb.Services.Answers;

using AutoMapper;
using PromoWeb.Context.Entities;

public class AnswerResponse
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

public class AnswerResponseProfile : Profile
{
    public AnswerResponseProfile()
    {
        CreateMap<AnswerModel, AnswerResponse>();
    }
}
