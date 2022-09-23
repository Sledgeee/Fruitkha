using System;
using System.Text;
using System.Threading.Tasks;
using Fruitkha.Core.Dtos.User;
using Fruitkha.Core.Entities;
using Fruitkha.Core.Entities.Ardalis;
using Fruitkha.Core.Exceptions;
using Fruitkha.Core.Helpers.Mails;
using Fruitkha.Core.Helpers.Mails.ViewModels;
using Fruitkha.Core.Interfaces;
using Fruitkha.Core.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Fruitkha.Core.Helpers;

namespace Fruitkha.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IConfirmEmailService _confirmEmailService;
        private readonly ITemplateService _templateService;
        private readonly IOptions<ClientUrl> _clientUrl;

        public AuthenticationService(
            UserManager<User> userManager,
            IJwtService jwtService,
            RoleManager<IdentityRole> roleManager,
            IRepository<RefreshToken> refreshTokenRepository,
            IEmailSenderService emailSenderService,
            IConfirmEmailService confirmEmailService,
            ITemplateService templateService,
            IOptions<ClientUrl> clientUrl)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
            _refreshTokenRepository = refreshTokenRepository;
            _emailSenderService = emailSenderService;
            _confirmEmailService = confirmEmailService;
            _templateService = templateService;
            _clientUrl = clientUrl;
        }

        public async Task<UserAuthDto> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new HttpException(System.Net.HttpStatusCode.Unauthorized, ErrorMessages.IncorrectLoginOrPassword);
            }

            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                return await GenerateTwoStepVerificationCode(user);
            }

            return await GenerateUserTokens(user);
        }

        private async Task<UserAuthDto> GenerateUserTokens(User user)
        {
            var claims = _jwtService.SetClaims(user);

            var token = _jwtService.CreateToken(claims);
            var refreshToken = await CreateRefreshToken(user);

            var tokens = new UserAuthDto()
            {
                Token = token,
                RefreshToken = refreshToken
            };

            return tokens;
        }

        private async Task<string> CreateRefreshToken(User user)
        {
            var refreshToken = _jwtService.CreateRefreshToken();

            RefreshToken rt = new()
            {
                Token = refreshToken,
                UserId = user.Id
            };

            await _refreshTokenRepository.AddAsync(rt);
            await _refreshTokenRepository.SaveChangesAsync();

            return refreshToken;
        }

        private async Task<UserAuthDto> GenerateTwoStepVerificationCode(User user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);

            if (!providers.Contains("Email"))
            {
                throw new HttpException(System.Net.HttpStatusCode.Unauthorized, ErrorMessages.Invalid2StepVerification);
            }

            var twoFactorToken = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var message = new MailRequest
            {
                ToEmail = user.Email,
                Subject = "InTouch Authentication Code",
                Body = await _templateService.GetTemplateHtmlAsStringAsync("Mails/TwoFactorCode",
                    new UserToken()
                        { Token = twoFactorToken, UserName = user.UserName, Uri = _clientUrl.Value.ApplicationUrl })
            };

            await _emailSenderService.SendEmailAsync(message);

            return new UserAuthDto() { IsTwoStepVerificationRequired = true, Provider = "Email" };
        }

        public async Task<UserAuthDto> LoginTwoStepAsync(UserTwoFactorDto twoFactorDto)
        {
            var user = await _userManager.FindByEmailAsync(twoFactorDto.Email);
            user.UserNullChecking();

            var validVerification =
                await _userManager.VerifyTwoFactorTokenAsync(user, twoFactorDto.Provider, twoFactorDto.Token);

            if (!validVerification)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.InvalidTokenVerification);
            }

            return await GenerateUserTokens(user);
        }

        public async Task RegistrationAsync(User user, string password, string roleName)
        {
            user.CreateDate = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero);
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                StringBuilder errorMessage = new();
                foreach (var error in result.Errors)
                {
                    errorMessage.Append(error.Description + " ");
                }

                throw new HttpException(System.Net.HttpStatusCode.BadRequest, errorMessage.ToString());
            }

            if (await _roleManager.FindByNameAsync(roleName) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<UserAuthDto> RefreshTokenAsync(UserAuthDto userTokensDto)
        {
            var specification = new RefreshTokens.SearchRefreshToken(userTokensDto.RefreshToken);
            var refreshTokenFromDb = await _refreshTokenRepository.GetFirstBySpecAsync(specification);

            if (refreshTokenFromDb == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.InvalidToken);
            }

            var claims = _jwtService.GetClaimsFromExpiredToken(userTokensDto.Token);
            var newToken = _jwtService.CreateToken(claims);
            var newRefreshToken = _jwtService.CreateRefreshToken();

            refreshTokenFromDb.Token = newRefreshToken;
            await _refreshTokenRepository.UpdateAsync(refreshTokenFromDb);
            await _refreshTokenRepository.SaveChangesAsync();

            var tokens = new UserAuthDto()
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            };

            return tokens;
        }

        public async Task LogoutAsync(UserAuthDto userTokensDto)
        {
            var specification = new RefreshTokens.SearchRefreshToken(userTokensDto.RefreshToken);
            var refreshToken = await _refreshTokenRepository.GetFirstBySpecAsync(specification);

            if (refreshToken == null)
            {
                return;
            }

            await _refreshTokenRepository.DeleteAsync(refreshToken);
            await _refreshTokenRepository.SaveChangesAsync();
        }

        public async Task SentResetPasswordTokenAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            user.UserNullChecking();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedCode = Convert.ToBase64String(Encoding.Unicode.GetBytes(token));

            await _emailSenderService.SendEmailAsync(new MailRequest()
            {
                ToEmail = user.Email,
                Subject = "InTouch Reset Password",
                Body = await _templateService.GetTemplateHtmlAsStringAsync("Mails/ResetPassword",
                    new UserToken()
                    {
                        Token = encodedCode,
                        UserName = user.UserName,
                        Uri = _clientUrl.Value.ApplicationUrl
                    })
            });
        }

        public async Task ResetPasswordAsync(UserChangePasswordDto userChangePasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(userChangePasswordDto.Email);
            user.UserNullChecking();

            var decodedCode = _confirmEmailService.DecodeUnicodeBase64(userChangePasswordDto.Code);

            var result = await _userManager.ResetPasswordAsync(user, decodedCode, userChangePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.WrongResetPasswordCode);
            }
        }
    }
}