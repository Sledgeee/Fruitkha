using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Fruitkha.Core.Dtos.User;
using Fruitkha.Core.Entities;
using Fruitkha.Core.Exceptions;
using Fruitkha.Core.Helpers;
using Fruitkha.Core.Helpers.Mails;
using Fruitkha.Core.Helpers.Mails.ViewModels;
using Fruitkha.Core.Interfaces;
using Fruitkha.Core.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Fruitkha.Core.ApiModels;

namespace Fruitkha.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<User> _userRepository;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IMapper _mapper;
        private readonly IOptions<ImageSettings> _imageSettings;
        private readonly ITemplateService _templateService;
        private readonly IOptions<ClientUrl> _clientUrl;

        public UserService(
            UserManager<User> userManager,
            IRepository<User> userRepository,
            IEmailSenderService emailSenderService,
            IMapper mapper,
            IOptions<ImageSettings> imageSettings,
            ITemplateService templateService,
            IOptions<ClientUrl> clientUrl)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _emailSenderService = emailSenderService;
            _mapper = mapper;
            _imageSettings = imageSettings;
            _templateService = templateService;
            _clientUrl = clientUrl;
        }


        public async Task<UserPersonalInfoDto> GetUserPersonalInfoAsync(string userId)
        {
            var user = await _userRepository.GetByKeyAsync(userId);

            return _mapper.Map<UserPersonalInfoDto>(user);
        }

        public async Task ChangeInfoAsync(string userId, UserChangeInfoDto userChangeInfoDto)
        {
            var userObj = await _userManager.FindByNameAsync(userChangeInfoDto.UserName);

            if (userObj != null && userObj.Id != userId)
            {
                throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.UsernameAlreadyExists);
            }

            var user = await _userRepository.GetByKeyAsync(userId);

            _mapper.Map(userChangeInfoDto, user);

            await _userRepository.UpdateAsync(user);

            await _userManager.UpdateNormalizedUserNameAsync(user);

            await _userRepository.SaveChangesAsync();

            await Task.CompletedTask;
        }

        public async Task ChangeTwoFactorVerificationStatusAsync(string userId, UserChangeTwoFactorDto statusDto)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var isUserToken = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", statusDto.Token);

            if (!isUserToken)
            {
                throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.Invalid2FVCode);
            }

            var result =
                await _userManager.SetTwoFactorEnabledAsync(user, !await _userManager.GetTwoFactorEnabledAsync(user));

            if (!result.Succeeded)
            {
                throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.InvalidRequest);
            }

            await Task.CompletedTask;
        }

        public async Task SendTwoFactorCodeAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var twoFactorToken = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            var message = new MailRequest()
            {
                ToEmail = user.Email,
                Subject = "InTouch Two-Factor Code",
                Body = await _templateService.GetTemplateHtmlAsStringAsync("Mails/TwoFactorCode",
                    new UserToken()
                    {
                        Token = twoFactorToken,
                        UserName = user.UserName,
                        Uri = _clientUrl.Value.ApplicationUrl
                    })
            };

            await _emailSenderService.SendEmailAsync(message);

            await Task.CompletedTask;
        }

        public async Task SetPasswordAsync(string userId, UserSetPasswordDto userSetPasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (await _userManager.HasPasswordAsync(user))
            {
                throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.PasswordIsExist);
            }

            await _userManager.AddPasswordAsync(user, userSetPasswordDto.Password);

            await Task.CompletedTask;
        }

        public async Task<bool> CheckIsTwoFactorVerificationAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (!user.EmailConfirmed)
            {
                throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.EmailNotConfirm);
            }

            return await _userManager.GetTwoFactorEnabledAsync(user);
        }

        public async Task<bool> IsHavePasswordAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.HasPasswordAsync(user);
        }
    }
}