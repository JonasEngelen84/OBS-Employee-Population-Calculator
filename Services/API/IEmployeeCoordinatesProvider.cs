using Geolocation;
using System.Collections.Generic;

namespace OBS.Dashboard.Map.Services.API
{
    public interface IEmployeeCoordinatesProvider
    {
        List<Coordinate> EmployeesCoordinates { get; }
    }
}