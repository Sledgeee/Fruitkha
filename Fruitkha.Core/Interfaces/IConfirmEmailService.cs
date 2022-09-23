using System.Threading.Tasks;
using Fruitkha.Core.Dtos.User;

namespace Fruitkha.Core.Interfaces
{
    public interface IConfirmEmailService
    {
        Task SendConfirmMailAsync(string userId);
        Task ConfirmEmailAsync(string userId, UserConfirmEmailDto confirmEmailDto);
        string DecodeUnicodeBase64(string input);
    }
}