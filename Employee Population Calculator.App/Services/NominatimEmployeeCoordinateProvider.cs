using Employee_Population_Calculator.App.Services.API;
using Geolocation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Employee_Population_Calculator.App.Services
{
    public class NominatimEmployeeCoordinateProvider : IEmployeeCoordinatesProvider
    {
        private IEmployeeAddressesProvider EmployeeAddressesProvider { get; }

        private readonly ILogger<NominatimEmployeeCoordinateProvider> _logger;
        private DateTime _lastCalculationTime = DateTime.Now;
        private List<Coordinate> _employeeCoordinateCache = null;

        public NominatimEmployeeCoordinateProvider(
            IEmployeeAddressesProvider employeeAddressesProvider,
            ILogger<NominatimEmployeeCoordinateProvider> logger)
        {
            EmployeeAddressesProvider = employeeAddressesProvider;
            _logger = logger;
        }

        public List<Coordinate> EmployeesCoordinates
        {
            get
            {
                if (_employeeCoordinateCache == null || DateTime.Now.Subtract(_lastCalculationTime).TotalHours > 1)
                {
                    _employeeCoordinateCache = CalculateCoordinates();
                    _lastCalculationTime = DateTime.Now;
                }

                return _employeeCoordinateCache;
            }
        }

        private List<Coordinate> CalculateCoordinates()
        {
            //// TODO: Schnittstelle zu Nominatim benötigt Key!
            //var geoCoder = new ForwardGeocoder();

            var coordinates = new List<Coordinate>();

            foreach (var address in EmployeeAddressesProvider.EmployeesAddresses)
            {
                try
                {
                    //var request = new ForwardGeocodeRequest {queryString = address, DedupeResults = true};
                    //var results = geoCoder.Geocode(request).Result;

                    //if (results.Length < 1)
                    //{
                    //    _logger.LogWarning("No address conversion result for: " + address);
                    //    continue;
                    //};

                    //_logger.LogInformation("Address conversion successful for: " + address);

                    //var result = results[0];

                    //coordinates.Add(
                    //    new Coordinate(
                    //        result.Latitude,
                    //        result.Longitude
                    //    )
                    //);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to convert address: " + address, ex);
                }
            }
            return coordinates;
        }
    }
}