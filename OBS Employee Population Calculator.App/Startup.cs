using OBS_Employee_Population_Calculator.App.Extensions;
using OBS_Employee_Population_Calculator.App.Models;
using OBS_Employee_Population_Calculator.App.Services;
using OBS_Employee_Population_Calculator.App.Services.API;
using OBS_Employee_Population_Calculator.App.Services.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OBS.Stamm.Client.Api;
using System.Net.Http;
using AuthenticationService = OBS_Employee_Population_Calculator.App.Services.AuthenticationService;

namespace OBS_Employee_Population_Calculator.App
{
    /// <summary>
    /// Konfiguriert die Webanwendung und deren Services.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ConfigureServices registriert die Services der Anwendung.
        /// </summary>
        /// <param name="services">Service Collection für Dependency Injection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.OBS.Configuration.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            services.AddRazorPages();

            services.AddTransient<ICompanyInformationProvider, ConfigurationCompanyInformationProvider>();
            services.AddTransient<ICirclesInformationProvider, CirclesInformationProvider>();
            services.AddTransient<ICirclesPropertyProvider, ConfigurationCirclesPropertyProvider>();
            services.AddTransient<IEmployeeAddressesProvider, ConfigurationEmployeeAddressesProvider>();
            services.AddTransient<IEmployeeAddressesProvider, EmployeeObsStammAddressesProvider>();
            services.AddSingleton<HttpClient>();
            services.AddOptions();
            services.AddTransient<AuthenticationService>();

            // Laden der Konfigurationen aus appsettings.json
            services.Configure<ServicesConfiguration>(configuration.GetSection("Services"));
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("Authentication"));

            /// <summary>
            /// Registriert die `IPersonsApi`-Implementierung als Transienten Dienst.
            /// Diese API ermöglicht den Zugriff auf die Personen-Daten aus dem OBS-Stamm-System.
            /// </summary>
            /// <remarks>
            /// - Die `PersonsApi`-Instanz wird mit der `StammServiceUrl` aus der Konfiguration initialisiert.
            /// - Falls `StammServiceUrl` nicht gesetzt ist, wird eine alternative URL basierend auf `ServiceTemplateUrl` generiert.
            /// - Die Authentifizierung erfolgt über ein Access Token, das über den `AuthenticationService` geholt wird.
            /// - Die erhaltene `personsApi`-Instanz wird mit der authentifizierten Konfiguration versehen.
            /// </remarks>
            /// <param name="provider">Der Dienstanbieter, der für die Dependency Injection verwendet wird.</param>
            services.AddTransient<IPersonsApi>(provider =>
            {
                var servicesConfig = provider.GetRequiredService<IOptions<ServicesConfiguration>>();
                var obsStammUrl = servicesConfig.Value.StammServiceUrl ?? servicesConfig.Value.ServiceTemplateUrl.Interpolate("service", "obsstamm");
                var personsApi = new PersonsApi($"{obsStammUrl}");

                // Authentifizierung über AccessToken
                var authenticationService = provider.GetRequiredService<AuthenticationService>();
                var accessToken = authenticationService.GetAccessTokenAsync(default).Result;
                personsApi.Configuration = OBS.Stamm.Client.Client.Configuration.MergeConfigurations(
                    new OBS.Stamm.Client.Client.Configuration()
                    {
                        AccessToken = accessToken
                    },
                    personsApi.Configuration);

                return personsApi;
            });

            // Konfiguration des EmployeeCoordinatesProvider basierend auf appsettings.json
            if (configuration.GetValue<CoordinateSource>("CoordinateSource") == CoordinateSource.Nominatim)
            {
                // Nominatim
                services.AddTransient<IEmployeeCoordinatesProvider, NominatimEmployeeCoordinateProvider>();
            }
            else
            {
                // appsettings.json
                services.AddTransient<IEmployeeCoordinatesProvider, ConfigurationEmployeeCoordinatesProvider>();
            }

            // Konfiguration des EmployeeAddressesProvider basierend auf appsettings.json
            if (configuration.GetValue<EmployeeAddressesSource>("EmployeeAddressesSource") == EmployeeAddressesSource.OBSStamm)
            {
                // OBSStamm
                services.AddTransient<IEmployeeAddressesProvider, EmployeeObsStammAddressesProvider>();
            }
            else
            {
                // appsettings.json
                services.AddTransient<IEmployeeAddressesProvider, ConfigurationEmployeeAddressesProvider>();
            }
        }


        /// <summary>
        /// Konfiguriert die HTTP-Request-Pipeline der Anwendung.
        /// </summary>
        /// <remarks>
        /// Die HTTP-Request-Pipeline ist eine Kette von Middleware-Komponenten,
        /// die eine eingehende HTTP-Anfrage verarbeiten.
        /// </remarks>
        /// <param name="app">Anwendungsbuilder</param>
        /// <param name="env">Webhosting-Umgebung</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Konfiguriert die Fehlerbehandlung basierend auf der Umgebung (Development/Production).
            if (env.IsDevelopment())
            {
                // Zeigt detaillierte Fehlerseiten an, wenn sich die Anwendung im Entwicklungsmodus befindet.
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Leitet auf eine generische Fehlerseite um, wenn ein Fehler in der Produktion auftritt.
                app.UseExceptionHandler("/Error");

                // Der Standard HSTS Wert beträgt 30 Tage. Wenn diese in production scenarios geändert werden soll, siehe: https://aka.ms/aspnetcore-hsts.
                // Erzwingt HTTPS-Verbindungen und schützt vor Man-in-the-Middle-Angriffen.
                app.UseHsts();
            }

            app.UseHttpsRedirection(); // Leitet alle HTTP-Anfragen auf HTTPS um.
            app.UseRouting(); // Aktiviert die Routing-Funktionalität, die bestimmt, welche Endpunkte für Anfragen zuständig sind.
            app.UseAuthorization(); // Aktiviert die Autorisierung, um Zugriffskontrollen durchzuführen.

            // Konfiguriert die Endpunkte der Anwendung, z. B. Razor Pages oder API-Controller.
            app.UseEndpoints(endpoints =>
            {
                // Aktiviert Razor Pages als Endpunkte für die Webanwendung.
                endpoints.MapRazorPages();
            });
        }
    }
}
