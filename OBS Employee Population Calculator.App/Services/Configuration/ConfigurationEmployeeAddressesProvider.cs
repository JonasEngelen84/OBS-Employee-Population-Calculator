using OBS_Employee_Population_Calculator.App.Services.API;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace OBS_Employee_Population_Calculator.App.Services.Configuration
{
    // Stellt eine Liste von Mitarbeiteradressen aus appsettings bereit.
    public class ConfigurationEmployeeAddressesProvider : IEmployeeAddressesProvider
    {
        public IConfiguration Configuration { get; } // Zugriff auf appsettings.json.

        public ConfigurationEmployeeAddressesProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Liefert alle konfigurierten Mitarbeiteradressen.
        public List<string> EmployeesAddresses
        {
            get
            {
                var addresses = new List<string>();

                // Iteriert über alle untergeordneten Einträge von "EmployeeAddresses"
                Configuration.GetSection("EmployeeAddresses").GetChildren().ToList().ForEach(employeeConfiguration =>
                {
                    addresses.Add(employeeConfiguration.Value);
                });

                return addresses;
            }
        }
    }
}
