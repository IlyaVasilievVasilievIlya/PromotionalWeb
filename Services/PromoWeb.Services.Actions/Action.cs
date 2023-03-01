using PromoWeb.Consts;
using PromoWeb.Services.EmailSender;
using PromoWeb.Services.RabbitMq;

namespace PromoWeb.Services.Actions
{
    public class Action : IAction
    {
        private readonly IRabbitMq rabbitMq;

        public Action(IRabbitMq rabbitMq)
        {
            this.rabbitMq = rabbitMq;
        }

        public async Task SendEmail(EmailModel email)
        {
            await rabbitMq.PushAsync(RabbitMqTaskQueueNames.SEND_EMAIL, email); //зачем consts
        }
    }
}
