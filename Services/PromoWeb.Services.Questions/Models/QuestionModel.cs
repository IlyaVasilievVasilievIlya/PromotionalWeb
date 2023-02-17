namespace PromoWeb.Services.Questions;

using AutoMapper;
using PromoWeb.Context.Entities;

public class QuestionModel
{
    public int Id { get; set; }

    public DateTime Date { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    //public string RecipientEmail { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}

public class QuestionModelProfile : Profile
{
    public QuestionModelProfile()
    {
        CreateMap<Question, QuestionModel>()
            .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => (src.Answer == null) ? "" : src.Answer.Text));
    }
}
