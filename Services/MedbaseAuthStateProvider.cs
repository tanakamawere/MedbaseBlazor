using MedbaseLibrary.Helpers;
using MedbaseLibrary.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedbaseBlazor.Services;

public class MedbaseAuthStateProvider : AuthenticationStateProvider
{
    private readonly IAuthMemory _authMemory;

    public MedbaseAuthStateProvider(IAuthMemory authMemory)
    {
        _authMemory = authMemory;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Your custom logic here (e.g., validate password, retrieve claims)
        //var token = await _localStorage.GetObjectAsync<string>("auth");
        var token = _authMemory.GetToken(Helpers.AuthMemory);
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
