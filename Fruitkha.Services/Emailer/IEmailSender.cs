using Fruitkha.Services.Emailer.Models;

namespace Fruitkha.Services.Emailer
{
    public interface IEmailSender
    {
        string SendEmail(SendEmailVM vm, MailSettings settings);
    }
}