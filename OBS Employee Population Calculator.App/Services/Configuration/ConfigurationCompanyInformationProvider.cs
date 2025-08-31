using OBS_Employee_Population_Calculator.App.Services.API;
using Geolocation;
using Microsoft.Extensions.Configuration;

namespace OBS_Employee_Population_Calculator.App.Services.Configuration
{
    // Stellt Unternehmens-Adresse und -Koordinaten aus appsettings bereit.
    public class ConfigurationCompanyInformationProvider : ICompanyInformationProvider
    {   
        public IConfiguration Configuration { get; } // Zugriff auf appsettings.json.

        public ConfigurationCompanyInformationProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Liefert die Koordinaten des Unternehmen.
        public Coordinate CompanyCoordinate =>
            new Coordinate(Configuration
                .GetSection("Company")
                .GetValue<double>("Latitude"), Configuration
                .GetSection("Company")
                .GetValue<double>("Longitude"));

        // Liefert die Adresse des Unternehmen.
        public string CompanyAddress =>
            Configuration
            .GetSection("Company")
            .GetValue<string>("Address");
    }
}
