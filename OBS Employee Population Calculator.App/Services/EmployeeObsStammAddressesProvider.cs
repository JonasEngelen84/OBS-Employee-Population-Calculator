using OBS_Employee_Population_Calculator.App.Services.API;
using OBS.Stamm.Client.Api;
using OBS.Stamm.Client.Model;
using System;
using System.Collections.Generic;

namespace OBS_Employee_Population_Calculator.App.Services
{
    /// <summary>
    /// Liefert die Adressen der Mitarbeiter aus dem OBS-Stammdatensystem.
    /// 
    /// Diese Klasse ruft das angebundene <see cref="IPersonsApi"/> auf,
    /// um für jede Person die Detailinformationen zu erhalten und daraus
    /// eine vollständige Adresse zu bauen.
    /// 
    /// Ergebnisse werden aus Performance-Gründen für 1 Stunde gecached.
    /// </summary>
    public class EmployeeObsStammAddressesProvider : IEmployeeAddressesProvider
    {
        private readonly IPersonsApi _api; //API-Schnittstelle zum OBS-Personensystem.
        private DateTime _lastCalculationTime = DateTime.Now; // Zeitpunkt der letzten Berechnung, steuert die Cache-Gültigkeit (1 Stunde).
        private List<string> _employeeAddressesCache = null; // Cache für die zuletzt abgerufenen Mitarbeiteradressen.

        public EmployeeObsStammAddressesProvider(IPersonsApi api)
        {
            _api = api;
        }

        // Liefert die Liste der Mitarbeiteradressen zurück.
        public List<string> EmployeesAddresses
        {
            get
            {
                // Falls keine Daten im Cache liegen oder die letzte Berechnung länger
                // als 1 Stunde her ist, werden die Adressen neu aus der API geladen.
                if (_employeeAddressesCache == null || DateTime.Now.Subtract(_lastCalculationTime).TotalHours > 1)
                {
                    _employeeAddressesCache = CalculateAddresses();
                    _lastCalculationTime = DateTime.Now;
                }

                return _employeeAddressesCache;
            }
        }

        /// <summary>
        /// Holt die Adressen aller Mitarbeiter aus der OBS-API.
        /// 
        /// Ablauf:
        /// 1. API liefert eine Liste von Personen-Deskriptoren.
        /// 2. Für jeden Deskriptor werden Detaildaten geladen.
        /// 3. Aus Straße, Postleitzahl und Ort wird eine vollständige Adresse als String zusammengesetzt.
        /// 4. Ergebnisse werden in einer Liste gesammelt.
        /// 
        /// Fehler werden abgefangen und in der Konsole ausgegeben.
        /// </summary>
        private List<string> CalculateAddresses()
        {
            List<string> employeeDetails = new List<string>();

            try
            {   
                foreach (var descriptor in _api.Descriptors())
                {
                    DetailedPersonApiModel apiDetails = _api.Details(descriptor.Id);

                    string address = apiDetails.StreetAndHouseNumber + " " + apiDetails.PostalCode + " " + apiDetails.CityAndCountry;
                    
                    employeeDetails.Add(address);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return employeeDetails;
        }
    }
}

