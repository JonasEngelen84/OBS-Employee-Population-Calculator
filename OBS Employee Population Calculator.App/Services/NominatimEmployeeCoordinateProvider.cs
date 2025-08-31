using OBS_Employee_Population_Calculator.App.Services.API;
using Geolocation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace OBS_Employee_Population_Calculator.App.Services
{
    /// <summary>
    /// Liefert über Nominatim die Geokoordinaten aller Mitarbeiter auf Basis ihrer Adressen.
    /// Die Ergebnisse werden zur Performance-Optimierung für 1 Stunde im Speicher gecached,
    /// sodass wiederholte Zugriffe nicht erneut eine Berechnung oder API-Abfrage starten.
    /// </summary>
    public class NominatimEmployeeCoordinateProvider : IEmployeeCoordinatesProvider
    {
        private IEmployeeAddressesProvider EmployeeAddressesProvider { get; } // Liefert die Adressen der Mitarbeiter.
        private readonly ILogger<NominatimEmployeeCoordinateProvider> _logger; // Logger-Instanz für Warnungen und Fehlermeldungen.
        private DateTime _lastCalculationTime = DateTime.Now; // Zeitpunkt, an dem zuletzt Koordinaten berechnet wurden.
        private List<Coordinate> _employeeCoordinateCache = null; // Cache für die berechneten Mitarbeiterkoordinaten.

        public NominatimEmployeeCoordinateProvider(
            IEmployeeAddressesProvider employeeAddressesProvider,
            ILogger<NominatimEmployeeCoordinateProvider> logger)
        {
            EmployeeAddressesProvider = employeeAddressesProvider;
            _logger = logger;
        }

        // Gibt die Liste der Mitarbeiterkoordinaten zurück.
        public List<Coordinate> EmployeesCoordinates
        {
            get
            {
                // Falls noch keine Berechnung erfolgt ist oder die letzte Berechnung länger als 1 Stunde her ist,
                // werden die Koordinaten neu berechnet.
                if (_employeeCoordinateCache == null || DateTime.Now.Subtract(_lastCalculationTime).TotalHours > 1)
                {
                    _employeeCoordinateCache = CalculateCoordinates();
                    _lastCalculationTime = DateTime.Now;
                }

                return _employeeCoordinateCache;
            }
        }

        /// <summary>
        /// Führt die eigentliche Umwandlung von Mitarbeiteradressen in Geokoordinaten durch.
        /// 
        /// Der Code für die Kommunikation mit Nominatim ist aktuell noch auskommentiert,
        /// da ein API-Key benötigt wird. Der Ablauf wäre:
        /// 1. Anfrage an Nominatim mit Adresse.
        /// 2. Prüfen, ob Ergebnisse vorhanden.
        /// 3. Erste Trefferkoordinate übernehmen.
        /// 4. Fehler oder keine Ergebnisse → Logging.
        /// </summary>
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