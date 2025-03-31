using Microsoft.Extensions.Configuration;
using OBS.Dashboard.Map.Models;
using OBS.Dashboard.Map.Services.API;
using System.Collections.Generic;
using System.Linq;

namespace OBS.Dashboard.Map.Services.Configuration
{
    public class ConfigurationCirclesPropertyProvider : ICirclesPropertyProvider
    {
        public IConfiguration Configuration { get; }

        public ConfigurationCirclesPropertyProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<PopulationCircleConfiguration> Configurations
        {
            get
            {
                var coordinates = new List<PopulationCircleConfiguration>();

                Configuration.GetSection("Circles").GetChildren().ToList().ForEach(

                    circleConfig =>
                    {
                        var config = new PopulationCircleConfiguration();
                        circleConfig.Bind(config);
                        coordinates.Add(config);
                    }
                );

                return coordinates;
            }
        }
    }
}
