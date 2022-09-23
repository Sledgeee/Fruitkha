using Ardalis.Specification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Fruitkha.Core.Entities.Ardalis
{
    public class RefreshTokens
    {
        internal class SearchRefreshToken : Specification<RefreshToken>
        {
            public SearchRefreshToken(string refreshToken)
            {
                Query.Where(x => x.Token == refreshToken);
            }
        }
    }
}