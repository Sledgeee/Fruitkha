using System.Threading.Tasks;
using Fruitkha.Core.Dtos.User;
using Microsoft.AspNetCore.Http;
using Fruitkha.Core.ApiModels;

namespace Fruitkha.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserPersonalInfoDto> GetUserPersonalInfoAsync(string userId);
        Task ChangeInfoAsync(string userId, UserChangeInfoDto userChangeInfoDto);
        Task ChangeTwoFactorVerificationStatusAsync(string userId, UserChangeTwoFactorDto statusDto);
        Task<bool> CheckIsTwoFactorVerificationAsync(string userId);
        Task SendTwoFactorCodeAsync(string userId);
        Task SetPasswordAsync(string userId, UserSetPasswordDto userSetPasswordDto);
        Task<bool> IsHavePasswordAsync(string userId);
    }
}