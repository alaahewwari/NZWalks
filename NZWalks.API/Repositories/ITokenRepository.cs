using Microsoft.AspNetCore.Identity;
using System.Data.Common;

namespace NZWalks.API.Repositories
{
    public interface ITokenRepository
    {
        //Create JWT Token
        string CreateJWTToken(IdentityUser user,List<string> roles);
    }
}
