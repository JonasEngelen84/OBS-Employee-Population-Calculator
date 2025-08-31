using OBS_Employee_Population_Calculator.App.Models;
using OBS_Employee_Population_Calculator.App.Services.API;
using Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OBS_Employee_Population_Calculator.App.Services
{
    /// <summary>
    /// Liefert die berechneten PopulationCircles.
    /// Verknüpft Mitarbeiterkoordinaten mit Kreis-Konfigurationen und
    /// berechnet anhand der Entfernungen die Radien.
    /// </summary>
    public class CirclesInformationProvider : ICirclesInformationProvider
    {
        // Abhängigkeiten werden über DI injiziert
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

                // Hole die Kreis-Konfigurationen und filtere nur valide Prozentwerte (0 < x ≤ 100).
                var circleConfigs = CirclesPropertyProvider.Configurations
                    .Where(config => config.PercentageOfEmployees <= 100 && config.PercentageOfEmployees > 0)
                    .OrderByDescending(config => config.PercentageOfEmployees);

                // Sortiere alle Mitarbeiter nach Distanz zum Firmenstandort.
                var sortedEmployeeCoordinates = SortEmployeesByRange(
                    EmployeeCoordinatesProvider.EmployeesCoordinates,
                    CompanyInformationProvider.CompanyCoordinate);

                // Erzeuge die PopulationCircles: Jeder Circle repräsentiert einen Prozentsatz an Mitarbeitern.
                List<PopulationCircle> circles = new List<PopulationCircle>();

                foreach (var config in circleConfigs)
                {
                    // Berechne den Index des Mitarbeiters, der für diesen Prozentsatz herangezogen wird
                    int employeeIndex = (int)Math.Max(
                        0,
                        (sortedEmployeeCoordinates.Count * config.PercentageOfEmployees / 100) - 1
                    );

                    // Hole die Koordinate des entsprechenden Mitarbeiters
                    var employeeCoordinate = sortedEmployeeCoordinates[employeeIndex];

                    // Berechne die Distanz vom Firmenstandort bis zu diesem Mitarbeiter
                    double distanceInMeters = GeoCalculator.GetDistance(
                        companyCoordinate,
                        employeeCoordinate,
                        0,
                        DistanceUnit.Meters
                    );

                    // Erzeuge den Kreis
                    var circle = new PopulationCircle(
                        config.ColorHex,
                        (int)distanceInMeters,
                        config.TooltipContents
                    );

                    circles.Add(circle);
                }

                return circles;
            }
        }

        // Sortiert alle Mitarbeiter nach Distanz zum Firmenstandort.
        private static List<Coordinate> SortEmployeesByRange(List<Coordinate> list, Coordinate company)
        {
            return list.OrderBy(c => Range(company, c)).ToList();
        }

        // Berechnet die Distanz zwischen einem Mitarbeiter und dem Unternehmen.
        private static double Range(Coordinate company, Coordinate employee)
        {
            return GeoCalculator.GetDistance(company, employee, 3, DistanceUnit.Meters);
        }
    }
}
