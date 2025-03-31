using Geolocation;
using Microsoft.Extensions.Configuration;
using OBS.Dashboard.Map.Services.API;

namespace OBS.Dashboard.Map.Services.Configuration
{
    public class ConfigurationCompanyInformationProvider : ICompanyInformationProvider
    {   
        public IConfiguration Configuration { get; }

        public ConfigurationCompanyInformationProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Coordinate CompanyCoordinate =>
            new Coordinate(Configuration
                .GetSection("Company")
                .GetValue<double>("Latitude"), Configuration
                .GetSection("Company")
                .GetValue<double>("Longitude"));


        public string CompanyAddress =>
            Configuration
            .GetSection("Company")
            .GetValue<string>("Address");
    }
}
