using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using OBS.Dashboard.Map.Services.API;

namespace OBS.Dashboard.Map.Services.Configuration
{
    public class ConfigurationEmployeeAddressesProvider : IEmployeeAddressesProvider
    {
        public IConfiguration Configuration { get; }

        public ConfigurationEmployeeAddressesProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<string> EmployeesAddresses
        {
            get
            {
                var addresses = new List<string>();

                Configuration.GetSection("EmployeeAddresses").GetChildren().ToList().ForEach(
                    employeeConfiguration =>
                    {
                        addresses.Add(employeeConfiguration.Value);
                    });

                return addresses;
            }
        }
    }
}
