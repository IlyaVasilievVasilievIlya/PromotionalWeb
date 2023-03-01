using PromoWeb.Services.EmailSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoWeb.Services.Actions
{
    public interface IAction
    {
        Task SendEmail(EmailModel email);
    }
}
