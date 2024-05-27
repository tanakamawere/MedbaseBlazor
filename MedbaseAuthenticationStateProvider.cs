using MedbaseLibrary.MsalClient;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedbaseBlazor;

public class MedbaseAuthenticationStateProvider : AuthenticationStateProvider
{
    protected MedbaseAuthenticationStateProvider()
    {
            
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Your custom logic here(e.g., validate password, retrieve claims)
        var token = GlobalValues.AccessToken;
        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtSecurityToken;

        jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var claims = jwtSecurityToken.Claims;

        var result = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "JwtBearer")));
        return result;
    }

    public string GetClaimValue(ClaimsPrincipal user, string claimType)
    {
        var claim = user.FindFirst(claimType);
        return claim?.Value;
    }
}
