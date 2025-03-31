using OBS.Dashboard.Map.Services.API;
using System;
using System.Collections.Generic;
using OBS.Stamm.Client.Api;
using OBS.Stamm.Client.Model;

namespace OBS.Dashboard.Map.Services
{
    public class EmployeeObsStammAddressesProvider : IEmployeeAddressesProvider
    {
        private readonly IPersonsApi api;
        private List<string> _employeeAddressesCache = null;
        private DateTime _lastCalculationTime = DateTime.Now;

        public EmployeeObsStammAddressesProvider(IPersonsApi api)
        {
            this.api = api;
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
                foreach (var descriptor in api.Descriptors())
                {
                    DetailedPersonApiModel apiDetails = api.Details(descriptor.Id);

                    string streetAndHouseNumber = apiDetails.StreetAndHouseNumber;
                    string postalCode = apiDetails.PostalCode;
                    string cityAndCountry = apiDetails.CityAndCountry;
                    string fullAddress = streetAndHouseNumber + " " + postalCode + " " + cityAndCountry;
                    
                    employeeDetails.Add(fullAddress);
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

