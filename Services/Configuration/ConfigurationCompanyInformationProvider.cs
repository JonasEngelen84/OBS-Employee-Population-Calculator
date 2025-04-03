using Employee_Population_Calculator.Services.API;
using Geolocation;
using Microsoft.Extensions.Configuration;

namespace Employee_Population_Calculator.Services.Configuration
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
