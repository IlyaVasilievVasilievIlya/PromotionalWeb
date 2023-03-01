

namespace PromoWeb.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings settings;

        public EmailSender(EmailSettings settings)
        {
            this.settings = settings;
        }

        public async Task Send(EmailModel model)
        {
            await new SMTPProvider(settings).SendEmailAsync(model.Email, model.Subject, model.Message);
        }
    }
}
