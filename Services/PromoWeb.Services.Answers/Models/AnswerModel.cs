namespace PromoWeb.Services.Answers;

using AutoMapper;
using PromoWeb.Context.Entities;

public class AnswerModel
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

public class AnswerModelProfile : Profile
{
    public AnswerModelProfile()
    {
        CreateMap<Answer, AnswerModel>()
            .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question.Text));
    }
}
