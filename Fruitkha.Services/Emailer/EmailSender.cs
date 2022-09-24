using System.Threading.Tasks;
using Fruitkha.Services.Emailer.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Fruitkha.Services.Emailer
{
    public class EmailSender : IEmailSender
    { 
        public string SendEmail(SendEmailVM vm, MailSettings settings)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(settings.Email));
            email.To.Add(MailboxAddress.Parse(vm.To));
            email.Subject = vm.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = vm.Message };

            using var smtp = new SmtpClient();
            smtp.Connect(settings.Smtp, settings.Port, false);
            smtp.Authenticate(settings.Email, settings.Password);
            var result = smtp.Send(email);
            smtp.Disconnect(true);
            smtp.Dispose();
            return result;
        }
    }
}