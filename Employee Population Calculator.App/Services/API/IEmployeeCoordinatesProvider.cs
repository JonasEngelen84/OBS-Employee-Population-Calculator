using Geolocation;
using System.Collections.Generic;

namespace Employee_Population_Calculator.App.Services.API
{
    public interface IEmployeeCoordinatesProvider
    {
        List<Coordinate> EmployeesCoordinates { get; }
    }
}