using MedbaseLibrary.Helpers;
using MedbaseLibrary.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedbaseBlazor;

public class MedbaseAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthMemory authMemory;
    private readonly ProtectedSessionStorage protectedSessionStorage;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    public MedbaseAuthenticationStateProvider(IAuthMemory authMemory, ProtectedSessionStorage protectedSession)
    {
        this.authMemory = authMemory;
        protectedSessionStorage = protectedSession;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionStorageResult = await protectedSessionStorage.GetAsync<string>(Helpers.AuthMemoryName);
            var token = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

            AuthenticationState result = new(new ClaimsPrincipal(new ClaimsIdentity()));

            //token = authMemory.GetToken().Result;
            if (string.IsNullOrEmpty(token))
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(TokenToClaims(token), "JwtBearer"));
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public async Task UpdateAuthenticationState(string token)
    {
        ClaimsPrincipal claimsPrincipal;

        if (token != null)
        {
            //User has logged in
            await protectedSessionStorage.SetAsync(Helpers.AuthMemoryName, token);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(TokenToClaims(token), "JwtBearer"));
        }
        else
        {
            //User has logged out
            await protectedSessionStorage.DeleteAsync(Helpers.AuthMemoryName);
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
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
