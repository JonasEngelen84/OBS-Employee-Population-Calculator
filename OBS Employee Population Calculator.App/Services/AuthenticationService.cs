using OBS_Employee_Population_Calculator.App.Extensions;
using OBS_Employee_Population_Calculator.App.Services.Configuration;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace OBS_Employee_Population_Calculator.App.Services
{
    /// <summary>
    /// Service, der für die Authentifizierung zuständig ist.
    /// Er kommuniziert mit einem Security Token Service (STS),
    /// um einen AccessToken zu beziehen, der dann für API-Aufrufe (OBS.Stamm) verwendet wird.
    ///
    /// Implementiert den Client-Credentials-Flow gemäß OAuth2/OpenID Connect.
    /// </summary>
    public class AuthenticationService
    {
        private readonly IOptions<AuthenticationConfiguration> _authConfig; // Injected Konfiguration für Authentifizierung (ClientId, ClientSecret, Scopes).
        private readonly IOptions<ServicesConfiguration> _servicesConfig; // Injected Konfiguration für Service-URLs (z. B. STS-Service-URL).
        private readonly ILogger<AuthenticationService> _logger; // Logging-Interface, um Ablauf und Fehler zu protokollieren.

        public AuthenticationService(
            ILogger<AuthenticationService> logger,
            IOptions<AuthenticationConfiguration> authConfig,
            IOptions<ServicesConfiguration> servicesConfig)
        {
            _logger = logger;
            _authConfig = authConfig;
            _servicesConfig = servicesConfig;
        }

        // Holt ein AccessToken asynchron vom STS.
        public async Task<string> GetAccessTokenAsync(CancellationToken token)
        {
            _logger.LogInformation("Starting request for authentication token from STS.");

            // STS-URL bestimmen: Entweder direkt gesetzt oder dynamisch interpoliert.
            var obsIdentityUrl = _servicesConfig.Value.STSServiceUrl
                ?? _servicesConfig.Value.ServiceTemplateUrl.Interpolate("service", "obsidentity");

            _logger.LogInformation($"Contacting STS Server at: '{obsIdentityUrl}' for OpenID Configuration.");

            // HttpClient anlegen und Discovery-Dokument abfragen (OIDC-Metadaten).
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync($"{obsIdentityUrl}", cancellationToken: token);

            // Falls keine OpenID-Konfiguration abrufbar → Abbruch.
            if (disco.IsError)
            {
                _logger.LogError("Failed to retrieve the discovery document. Throwing failure back to caller.");
                throw new AuthenticationException(disco.Error);
            }

            _logger.LogInformation($"Discovery document found. Requesting credentials for Client: '{_authConfig.Value.ClientId}'" +
                $" with scopes: '{_authConfig.Value.Scopes.Aggregate((l, r) => $"{l}, {r}")}'");

            // Client-Credentials-Flow: Anfrage an TokenEndpoint mit ClientId, Secret und Scope.
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = _authConfig.Value.ClientId,
                    ClientSecret = _authConfig.Value.ClientSecret,
                    Scope = _authConfig.Value.Scopes.Aggregate((l, r) => $"{l} {r}")
                },
                cancellationToken: token);

            // Falls Token nicht erhalten → Fehler werfen.
            if (tokenResponse.IsError)
            {
                _logger.LogError("Failed to request credentials. An authentication error has occured. Throwing failure back to caller.");
                throw new InvalidCredentialException(tokenResponse.Error);
            }

            _logger.LogInformation($"Authentication token received. It expires in: {tokenResponse.ExpiresIn} seconds.");

            // Bei Erfolg → AccessToken zurückgeben.
            return tokenResponse.AccessToken;
        }
    }
}

