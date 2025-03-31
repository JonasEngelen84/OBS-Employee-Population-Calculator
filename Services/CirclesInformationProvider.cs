using System;
using System.Collections.Generic;
using System.Linq;
using Geolocation;
using OBS.Dashboard.Map.Models;
using OBS.Dashboard.Map.Services.API;

namespace OBS.Dashboard.Map.Services
{
    public class CirclesInformationProvider : ICirclesInformationProvider
    {
        private ICirclesPropertyProvider CirclesPropertyProvider { get; }
        private IEmployeeCoordinatesProvider EmployeeCoordinatesProvider { get; }
        private ICompanyInformationProvider CompanyInformationProvider { get; }

        public CirclesInformationProvider(
            IEmployeeCoordinatesProvider employeeCoordinatesProvider,
            ICirclesPropertyProvider circlesPropertyProvider,
            ICompanyInformationProvider companyInformationProvider)
        {
            EmployeeCoordinatesProvider = employeeCoordinatesProvider;
            CirclesPropertyProvider = circlesPropertyProvider;
            CompanyInformationProvider = companyInformationProvider;
        }

        List<PopulationCircle> ICirclesInformationProvider.Circles
        {
            get
            {
                var companyCoordinate = CompanyInformationProvider.CompanyCoordinate;

                var circleConfigs = CirclesPropertyProvider.Configurations
                    .Where(config => config.PercentageOfEmployees <= 100 && config.PercentageOfEmployees > 0)
                    .OrderByDescending(config => config.PercentageOfEmployees);

                var sortedEmployeeCoordinates = SortEmployeesByRange(
                    EmployeeCoordinatesProvider.EmployeesCoordinates,
                    CompanyInformationProvider.CompanyCoordinate);

                List<PopulationCircle> circles = circleConfigs.Select(config => new PopulationCircle(
                    config.ColorHex,
                    (int)GeoCalculator.GetDistance(companyCoordinate,
                        sortedEmployeeCoordinates[
                            (int)Math.Max(0, (sortedEmployeeCoordinates.Count * config.PercentageOfEmployees / 100) - 1)], 0,
                        DistanceUnit.Meters),
                    config.TooltipContents
                )).ToList();

                return circles;
            }
        }

        private static double Range(Coordinate company, Coordinate employee)
        {
            return GeoCalculator.GetDistance(company, employee, 3, DistanceUnit.Meters);
        }

        private static List<Coordinate> SortEmployeesByRange(List<Coordinate> list, Coordinate company)
        {
            return list.OrderBy(c => Range(company, c)).ToList();
        }
    }
}
