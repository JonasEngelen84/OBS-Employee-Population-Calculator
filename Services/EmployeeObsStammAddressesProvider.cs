using OBS.Dashboard.Map.Services.API;
using System;
using System.Collections.Generic;
using OBS.Stamm.Client.Api;
using OBS.Stamm.Client.Model;

namespace OBS.Dashboard.Map.Services
{
    public class EmployeeObsStammAddressesProvider : IEmployeeAddressesProvider
    {
        private readonly IPersonsApi _api;
        private List<string> _employeeAddressesCache = null;
        private DateTime _lastCalculationTime = DateTime.Now;

        public EmployeeObsStammAddressesProvider(IPersonsApi api)
        {
            _api = api;
        }

        public List<string> EmployeesAddresses
        {
            get
            {   
                if (_employeeAddressesCache == null || DateTime.Now.Subtract(_lastCalculationTime).TotalHours > 1)
                {
                    _employeeAddressesCache = CalculateAddresses();
                    _lastCalculationTime = DateTime.Now;
                }

                return _employeeAddressesCache;
            }
        }

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

