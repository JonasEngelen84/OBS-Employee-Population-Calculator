using Employee_Population_Calculator.Services.API;
using Geolocation;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Employee_Population_Calculator.Services.Configuration
{
    public class ConfigurationEmployeeCoordinatesProvider : IEmployeeCoordinatesProvider
    {
        public IConfiguration Configuration { get; }

        public ConfigurationEmployeeCoordinatesProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<Coordinate> EmployeesCoordinates
        {
            get
            {
                var coordinates = new List<Coordinate>();

                Configuration.GetSection("EmployeeCoordinates").GetChildren().ToList().ForEach(
                    employeeConfiguration =>
                    {
                        var coordinate = new Coordinate(employeeConfiguration.GetValue<double>("Latitude"),
                            employeeConfiguration.GetValue<double>("Longitude"));
                        coordinates.Add(coordinate);
                    }
                );

                return coordinates;
            }
        }
    }
}
