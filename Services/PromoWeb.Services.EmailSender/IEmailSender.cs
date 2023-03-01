using PromoWeb.Services.EmailSender;

namespace PromoWeb.Services.EmailSender
{
    public interface IEmailSender
    {
        Task Send(EmailModel email);
    }
}
