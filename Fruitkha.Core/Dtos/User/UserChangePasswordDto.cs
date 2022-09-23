namespace Fruitkha.Core.Dtos.User
{
    public class UserChangePasswordDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Code { get; set; }
    }
}