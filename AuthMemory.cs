using MedbaseLibrary.Helpers;
using MedbaseLibrary.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MedbaseBlazor;

public class AuthMemory : IAuthMemory
{
    readonly ProtectedLocalStorage _localStorage;
    public AuthMemory(ProtectedLocalStorage protectedLocal)
    {
        _localStorage = protectedLocal;
    }
    public async Task<string> GetToken()
    {
        var result = await _localStorage.GetAsync<string>(Helpers.AuthMemoryName);
        return result.Value;
    }

    public async void RemoveToken(string userId)
    {
        await _localStorage.DeleteAsync(Helpers.AuthMemoryName);
    }

    public async void StoreToken(string userId, string jwt)
    {
        await _localStorage.SetAsync(Helpers.AuthMemoryName, jwt);
    }
}
