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
                }, Microsoft.Identity.Client.LogLevel.Always, enablePiiLogging: false, enableDefaultPlatformLogging: true)
                .Build();
        }
        public async Task<AuthenticationResult> AcquireTokenInteractiveAsync(string[] scopes)
        {
            if (CCA == null)
                return null;

            var accounts = await CCA.GetAccountsAsync(_settings?.PolicySignUpSignIn)
                .ConfigureAwait(false);
            var account = accounts.FirstOrDefault();

            return await CCA.AcquireTokenForClient(scopes)
                .ExecuteAsync().ConfigureAwait(false);
        }

        public Task<AuthenticationResult> AcquireTokenSilentAsync(string[] scopes)
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
