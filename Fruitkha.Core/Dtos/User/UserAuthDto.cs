namespace Fruitkha.Core.Dtos.User
{
    public class UserAuthDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Provider { get; set; }
        public bool IsTwoStepVerificationRequired { get; set; }
    }
}