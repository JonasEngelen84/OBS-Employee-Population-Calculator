using System.Collections.Generic;
using OBS.Dashboard.Map.Models;

namespace OBS.Dashboard.Map.Services.API
{
    public interface ICirclesPropertyProvider
    {
        List<PopulationCircleConfiguration> Configurations { get; }        
    }
}
