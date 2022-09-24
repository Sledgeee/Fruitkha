namespace Fruitkha.Services.Emailer.Models
{
    public class MailSettings
    {
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}