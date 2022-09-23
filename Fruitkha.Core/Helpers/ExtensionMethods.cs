using System.Net;
using Fruitkha.Core.Entities;
using Fruitkha.Core.Exceptions;
using Fruitkha.Core.Resources;

namespace Fruitkha.Core.Helpers
{
    public static class ExtensionMethods
    {
        public static void UserNullChecking(this User user)
        {
            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound,
                    ErrorMessages.UserNotFound);
            }
        }
    }
}
