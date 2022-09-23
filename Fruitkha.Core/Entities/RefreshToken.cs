namespace Fruitkha.Core.Entities
{
    public class RefreshToken : Base
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}