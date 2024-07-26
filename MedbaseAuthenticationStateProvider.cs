using MedbaseLibrary.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedbaseBlazor;

public class MedbaseAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthMemory authMemory;
    public MedbaseAuthenticationStateProvider(IAuthMemory authMemory)
    {
        this.authMemory = authMemory;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string token = "";
        AuthenticationState result = new(new ClaimsPrincipal(new ClaimsIdentity()));
        try
        {
            //token = authMemory.GetToken().Result;
            if (string.IsNullOrEmpty(token))
            {
                return result;
            }
            else
            {
                result = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(TokenToClaims(token), "JwtBearer")));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return result;
    }

    private IEnumerable<Claim> TokenToClaims(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtSecurityToken;

        jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var claims = jwtSecurityToken.Claims;

        return claims;
    }
}
