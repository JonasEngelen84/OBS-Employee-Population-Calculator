using Employee_Population_Calculator.Models;
using System.Collections.Generic;

namespace Employee_Population_Calculator.Services.API
{
    public interface ICirclesPropertyProvider
    {
        List<PopulationCircleConfiguration> Configurations { get; }        
    }
}
