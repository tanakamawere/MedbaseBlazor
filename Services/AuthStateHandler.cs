using MedbaseLibrary.Services;
using Blazored.LocalStorage;

namespace MedbaseBlazor.Services;

public class AuthStateHandler : ILocalStorage
{
    private readonly ILocalStorageService _localStorage;
    public AuthStateHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task ClearAsync()
    {
        await _localStorage.ClearAsync();
    }

    public async ValueTask<bool> ContainsKeyAsync(string name)
    {
        return await _localStorage.ContainKeyAsync(name);
    }

    public async Task<T> GetObjectAsync<T>(string name)
    {
        return await _localStorage.GetItemAsync<T>(name);
    }

    public async Task RemoveObjectAsync(string name)
    {
        await _localStorage.RemoveItemAsync(name);
    }

    public async Task SetObjectAsync(string name, object data)
    {
        await _localStorage.SetItemAsync(name, data);
    }
}
