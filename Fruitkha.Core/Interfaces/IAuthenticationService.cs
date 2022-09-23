using System.Threading.Tasks;
using Fruitkha.Core.Dtos.User;
using Fruitkha.Core.Entities;

namespace Fruitkha.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task RegistrationAsync(User user, string password, string roleName);
        Task<UserAuthDto> LoginAsync(string email, string password);
        Task<UserAuthDto> RefreshTokenAsync(UserAuthDto userTokensDto);
        Task LogoutAsync(UserAuthDto userTokensDto);
        Task<UserAuthDto> LoginTwoStepAsync(UserTwoFactorDto twoFactorDto);
        Task SentResetPasswordTokenAsync(string userEmail);
        Task ResetPasswordAsync(UserChangePasswordDto userChangePasswordDto);
    }
}