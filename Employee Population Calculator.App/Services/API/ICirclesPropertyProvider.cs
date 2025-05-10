using Employee_Population_Calculator.App.Models;
using System.Collections.Generic;

namespace Employee_Population_Calculator.App.Services.API
{
    public interface ICirclesPropertyProvider
    {
        List<PopulationCircleConfiguration> Configurations { get; }        
    }
}
