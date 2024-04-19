using MedbaseLibrary.MsalClient;
using MedbaseLibrary.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedbaseBlazor;

public class MedbaseAuthStateProvider : AuthenticationStateProvider
{ 

    public MedbaseAuthStateProvider()
    {
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Your custom logic here (e.g., validate password, retrieve claims)
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
}
