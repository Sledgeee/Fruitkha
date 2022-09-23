using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Fruitkha.Core.Dtos.User;
using Fruitkha.Core.Entities;
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
    public class ConfirmEmailService : IConfirmEmailService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ITemplateService _templateService;
        private readonly IOptions<ClientUrl> _clientUrl;

        public ConfirmEmailService(UserManager<User> userManager, IEmailSenderService emailSenderService,
            ITemplateService templateService, IOptions<ClientUrl> clientUrl)
        {
            _userManager = userManager;
            _emailSenderService = emailSenderService;
            _templateService = templateService;
            _clientUrl = clientUrl;
        }

        public async Task SendConfirmMailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            CheckUserAndEmailConfirmed(user);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedCode = Convert.ToBase64String(Encoding.Unicode.GetBytes(token));

            await _emailSenderService.SendEmailAsync(new MailRequest()
            {
                ToEmail = user.Email,
                Subject = "Velar Email Confirmation Code",
                Body = await _templateService.GetTemplateHtmlAsStringAsync("Mails/ConfirmEmail",
                    new UserToken()
                    {
                        Token = encodedCode,
                        UserName = user.UserName,
                        Uri = _clientUrl.Value.ApplicationUrl
                    })
            });

            await Task.CompletedTask;
        }

        private void CheckUserAndEmailConfirmed(User user)
        {
            user.UserNullChecking();

            if (user.EmailConfirmed)
            {
                throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.AlreadyConfirmEmail);
            }
        }

        public async Task ConfirmEmailAsync(string userId, UserConfirmEmailDto confirmEmailDto)
        {
            var user = await _userManager.FindByIdAsync(userId);

            CheckUserAndEmailConfirmed(user);

            var decodedCode = DecodeUnicodeBase64(confirmEmailDto.ConfirmationCode);

            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

            if (!result.Succeeded)
            {
                throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.ConfirmEmailInvalidCode);
            }

            await _userManager.UpdateSecurityStampAsync(user);

            await Task.CompletedTask;
        }

        public string DecodeUnicodeBase64(string input)
        {
            var bytes = new Span<byte>(new byte[input.Length]);

            if (!Convert.TryFromBase64String(input, bytes, out var bytesWritten))
            {
                throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.ConfirmEmailInvalidCode);
            }

            return Encoding.Unicode.GetString(bytes.Slice(0, bytesWritten));
        }
    }
}