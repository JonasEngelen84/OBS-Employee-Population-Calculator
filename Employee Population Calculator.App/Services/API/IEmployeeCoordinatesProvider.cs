using Geolocation;
using System.Collections.Generic;

namespace Employee_Population_Calculator.Services.API
{
    public interface IEmployeeCoordinatesProvider
    {
        List<Coordinate> EmployeesCoordinates { get; }
    }
}