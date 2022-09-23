namespace Fruitkha.Core.Dtos.User
{
    public class UserTwoFactorDto
    {
        public string Email { get; set; }
        public string Provider { get; set; }
        public string Token { get; set; }
    }
}