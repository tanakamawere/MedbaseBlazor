using MedbaseLibrary.MsalClient;
using Microsoft.Identity.Client;

namespace MedbaseBlazor
{
    public class CCAWrapper : IPCAWrapper
    {
        private readonly IConfiguration _configuration;
        private readonly Settings _settings;
        internal IConfidentialClientApplication CCA { get; }
        internal bool UseEmbedded { get; set; } = false;
        public string[] Scopes { get; set; }
        string[] myScopes = { "https://graphs.microsoft.com/.default" };

        //Public Constructor
        public CCAWrapper(IConfiguration configuration)
        {
            _configuration = configuration;
            _settings = _configuration.GetRequiredSection("Settings").Get<Settings>();

            CCA = ConfidentialClientApplicationBuilder
                .Create(_settings?.ClientId)
                .WithClientSecret(_settings?.ClientSecret)
                .WithB2CAuthority(_settings?.Authority)
                .WithLogging((level, message, containsPii) =>
                { 
                    Console.WriteLine($"[{level}] {message}");
                }, Microsoft.Identity.Client.LogLevel.Verbose, enablePiiLogging: true, enableDefaultPlatformLogging: true)
                .Build();
        }
        public async Task<AuthenticationResult> AcquireTokenInteractiveAsync(string[] scopes)
        {
            if (CCA == null)
                return null;

            var accounts = await CCA.GetAccountsAsync(_settings?.PolicySignUpSignIn)
                .ConfigureAwait(false);
            var account = accounts.FirstOrDefault();

            return await CCA.AcquireTokenForClient(myScopes)
                .ExecuteAsync().ConfigureAwait(false);
        }

        public async Task<AuthenticationResult> AcquireTokenSilentAsync(string[] scopes)
        {
            if (CCA == null)
                return null;

            var accounts = await CCA.GetAccountsAsync(_settings?.PolicySignUpSignIn).ConfigureAwait(false);
            var account = accounts.FirstOrDefault();

            var authResult = await CCA.AcquireTokenSilent(myScopes, account)
                                        .ExecuteAsync().ConfigureAwait(false);
            return authResult;
        }

        public async Task SignOutAsync()
        {
            if (CCA == null)
                return;

            var accounts = await CCA.GetAccountsAsync().ConfigureAwait(false);
            foreach (var account in accounts)
            {
                await CCA.RemoveAsync(account).ConfigureAwait(false);
            }
        }
    }
}
