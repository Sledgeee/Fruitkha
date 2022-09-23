using System.Collections.Generic;
using System.Security.Claims;
using Fruitkha.Core.Entities;

namespace Fruitkha.Core.Interfaces
{
    public interface IJwtService
    {
        IEnumerable<Claim> SetClaims(User user);
        string CreateToken(IEnumerable<Claim> claims);
        string CreateRefreshToken();
        IEnumerable<Claim> GetClaimsFromExpiredToken(string token);
    }
}