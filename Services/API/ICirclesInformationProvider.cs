using OBS.Dashboard.Map.Models;
using System.Collections.Generic;

namespace OBS.Dashboard.Map.Services.API
{
    public interface ICirclesInformationProvider
    {
        List<PopulationCircle> Circles { get; }
    }
}
