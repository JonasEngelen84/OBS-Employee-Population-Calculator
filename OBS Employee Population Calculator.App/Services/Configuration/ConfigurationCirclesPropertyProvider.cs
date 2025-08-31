using OBS_Employee_Population_Calculator.App.Models;
using OBS_Employee_Population_Calculator.App.Services.API;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace OBS_Employee_Population_Calculator.App.Services.Configuration
{
    // Stellt eine Liste von konfigurierten Kreisen aus appsettings bereit.
    public class ConfigurationCirclesPropertyProvider : ICirclesPropertyProvider
    {
        public IConfiguration Configuration { get; } // Zugriff auf appsettings.json.

        public ConfigurationCirclesPropertyProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Liefert die Liste der Circle-Konfigurationen, die in appsettings im Abschnitt "Circles" definiert sind.
        public List<PopulationCircleConfiguration> Configurations
        {
            get
            {
                var coordinates = new List<PopulationCircleConfiguration>();

                // Hole alle Unterabschnitte von "Circles" und binde sie an PopulationCircleConfiguration-Objekte.
                Configuration.GetSection("Circles").GetChildren().ToList().ForEach(circleConfig =>
                {
                    var config = new PopulationCircleConfiguration();
                    circleConfig.Bind(config);
                    coordinates.Add(config);
                });

                return coordinates;
            }
        }
    }
}
