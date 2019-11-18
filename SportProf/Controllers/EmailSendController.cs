using System.Net.Mail;
using System.Threading;

namespace SportProf.Controllers
{
    public static class EmailSendController
    {
        public static void SendEmail(string recipient, string subject, string body)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587) { Credentials = new System.Net.NetworkCredential("yevhenii.kovalov@nure.ua", "jDIJ2a7a"), EnableSsl = true };
            string testMail = "Test mail sended!";
            new Thread(() => client.SendAsync("yevhenii.kovalov@nure.ua", recipient, subject, body, testMail)).Start();
        }
    }
}