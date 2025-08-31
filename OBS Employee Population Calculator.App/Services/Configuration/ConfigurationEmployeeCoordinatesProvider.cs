using OBS_Employee_Population_Calculator.App.Services.API;
using Geolocation;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace OBS_Employee_Population_Calculator.App.Services.Configuration
{
    // Stellt eine Liste von Mitarbeiterkoordinaten aus appsettings bereit.
    public class ConfigurationEmployeeCoordinatesProvider : IEmployeeCoordinatesProvider
    {
        public IConfiguration Configuration { get; } // Zugriff auf appsettings.json.

        public ConfigurationEmployeeCoordinatesProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Liefert die Liste aller in der Konfiguration hinterlegten Mitarbeiterkoordinaten.
        public List<Coordinate> EmployeesCoordinates
        {
            get
            {
                var coordinates = new List<Coordinate>();

                // Durchläuft alle JSON-Child-Elemente von "EmployeeCoordinates"
                Configuration.GetSection("EmployeeCoordinates").GetChildren().ToList().ForEach(employeeConfiguration =>
                {
                    // Liest die Werte "Latitude" und "Longitude" und baut daraus ein Coordinate-Objekt
                    var coordinate = new Coordinate(employeeConfiguration.GetValue<double>("Latitude"),
                        employeeConfiguration.GetValue<double>("Longitude"));
                    coordinates.Add(coordinate);
                });

                return coordinates;
            }
        }
    }
}
