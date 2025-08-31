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
        /// <param name="services">Service Collection f�r Dependency Injection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            /// <summary>
            /// Erstellen einer Konfigurationsinstanz f�r die Anwendung.
            /// Die ConfigurationBuilder-Klasse erlaubt es, verschiedene Konfigurationsquellen 
            /// in einer hierarchischen Reihenfolge zusammenzuf�hren.
            /// Die Reihenfolge ist entscheidend: sp�tere Eintr�ge �berschreiben fr�here, falls Schl�ssel identisch sind.
            /// </summary>
            var configuration = new ConfigurationBuilder()

            // L�dt die Standardkonfigurationsdatei "appsettings.json".
            // - optional: true  ? wenn die Datei fehlt, wird kein Fehler geworfen.
            // - reloadOnChange: true ? �nderungen an der Datei w�hrend der Laufzeit
            //   werden automatisch erkannt und in die Configuration �bernommen.
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.OBS.Configuration.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            services.AddRazorPages(); // F�gt Razor Pages Support hinzu. Erm�glicht die Verwendung von .cshtml-Seiten + PageModel-Backends.
            services.AddOptions(); // Aktiviert die Options-Pattern-Unterst�tzung. Macht es m�glich, Konfigurationen typsicher via IOptions<T> zu binden.
            services.AddSingleton<HttpClient>(); // Registrierung eines HttpClient (Singleton, da ressourcenintensiv im Aufbau).

            // Alle Transient ? jeweils neue Instanz pro Request/Injection.
            // Das h�lt die Services leichtgewichtig und ohne Zustandsprobleme.
            services.AddTransient<AuthenticationService>();
            services.AddTransient<ICompanyInformationProvider, ConfigurationCompanyInformationProvider>();
            services.AddTransient<ICirclesInformationProvider, CirclesInformationProvider>();
            services.AddTransient<ICirclesPropertyProvider, ConfigurationCirclesPropertyProvider>();

            // Zwei Implementierungen f�r IEmployeeAddressesProvider.
            // - ConfigurationEmployeeAddressesProvider: liest Adressen aus appsettings.json
            // - ObsStammEmployeeAddressesProvider: liest Adressen aus OBS.Stamm.
            // Wichtig: Bei mehrfacher Registrierung wird im Standard nur die letzte aufgel�st.
            services.AddTransient<IEmployeeAddressesProvider, ConfigurationEmployeeAddressesProvider>();
            services.AddTransient<IEmployeeAddressesProvider, ObsStammEmployeeAddressesProvider>();

            // Laden der Service- & Auth-Konfigurationen aus appsettings.OBS.Configuration.json
            services.Configure<ServicesConfiguration>(configuration.GetSection("Services"));
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("Authentication"));

            /// <summary>
            /// Registriert die `IPersonsApi`-Implementierung als Transienten Dienst.
            /// Diese API erm�glicht den Zugriff auf die Personen-Daten aus dem OBS-Stamm-System.
            /// </summary>
            /// <remarks>
            /// - Die `PersonsApi`-Instanz wird mit der `StammServiceUrl` aus der Konfiguration initialisiert.
            /// - Falls `StammServiceUrl` nicht gesetzt ist, wird eine alternative URL basierend auf `ServiceTemplateUrl` generiert.
            /// - Die Authentifizierung erfolgt �ber ein Access Token, das �ber den `AuthenticationService` geholt wird.
            /// - Die erhaltene `personsApi`-Instanz wird mit der authentifizierten Konfiguration versehen.
            /// </remarks>
            /// <param name="provider">Der Dienstanbieter, der f�r die Dependency Injection verwendet wird.</param>
            services.AddTransient<IPersonsApi>(provider =>
            {
                var servicesConfig = provider.GetRequiredService<IOptions<ServicesConfiguration>>();
                var obsStammUrl = servicesConfig.Value.StammServiceUrl ?? servicesConfig.Value.ServiceTemplateUrl.Interpolate("service", "obsstamm");
                var personsApi = new PersonsApi($"{obsStammUrl}");

                // Authentifizierung �ber AccessToken
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
            // Schl�sselw�rter sind duch das enum CoordinateSource festgelegt.
            if (configuration.GetValue<CoordinateSource>("CoordinateSource") == CoordinateSource.Nominatim)
            {
                // Nominatim
                services.AddTransient<IEmployeeCoordinatesProvider, NominatimEmployeeCoordinateProvider>();
            }
            else
            {
                // appsettings
                services.AddTransient<IEmployeeCoordinatesProvider, ConfigurationEmployeeCoordinatesProvider>();
            }

            // Konfiguration des EmployeeAddressesProvider basierend auf appsettings.json
            // Schl�sselw�rter sind duch das enum EmployeeAddressesSource festgelegt.
            if (configuration.GetValue<EmployeeAddressesSource>("EmployeeAddressesSource") == EmployeeAddressesSource.OBSStamm)
            {
                // OBSStamm
                services.AddTransient<IEmployeeAddressesProvider, ObsStammEmployeeAddressesProvider>();
            }
            else
            {
                // appsettings
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

                // Der Standard HSTS Wert betr�gt 30 Tage. Wenn diese in production scenarios ge�ndert werden soll, siehe: https://aka.ms/aspnetcore-hsts.
                // Erzwingt HTTPS-Verbindungen und sch�tzt vor Man-in-the-Middle-Angriffen.
                app.UseHsts();
            }

            app.UseHttpsRedirection(); // Leitet alle HTTP-Anfragen auf HTTPS um.
            app.UseRouting(); // Aktiviert die Routing-Funktionalit�t, die bestimmt, welche Endpunkte f�r Anfragen zust�ndig sind.
            app.UseAuthorization(); // Aktiviert die Autorisierung, um Zugriffskontrollen durchzuf�hren.

            // Konfiguriert die Endpunkte der Anwendung, z. B. Razor Pages oder API-Controller.
            app.UseEndpoints(endpoints =>
            {
                // Aktiviert Razor Pages als Endpunkte f�r die Webanwendung.
                endpoints.MapRazorPages();
            });
        }
    }
}
