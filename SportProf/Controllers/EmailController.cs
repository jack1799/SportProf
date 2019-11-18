using ActionMailer.Net.Mvc;
using SportProf.Models;

namespace SportProf.Controllers
{
    public class EmailController : MailerBase
    {
        public EmailResult SendEmail(EmailModel model)
        {
            To.Add(model.To);

            From = model.From;

            Subject = model.Subject;

            return Email("SendEmail", model);
        }
    }
}
