using System.Threading.Tasks;
using Fruitkha.Core.Helpers.Mails;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Fruitkha.Core.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task<Response> SendEmailAsync(SendGridMessage message);
        Task SendManyMailsAsync<T>(MailingRequest<T> mailing) where T : class, new();
    }
}