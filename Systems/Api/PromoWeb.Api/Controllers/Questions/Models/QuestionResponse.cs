namespace PromoWeb.Api.Controllers;

using AutoMapper;
using PromoWeb.Services.Questions;

public class QuestionResponse
{
    public int Id { get; set; }

    public DateTime Date { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    //public string RecipientEmail { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}

public class QuestionResponseProfile : Profile
{
    public QuestionResponseProfile()
    {
        CreateMap<QuestionModel, QuestionResponse>();
    }
}
