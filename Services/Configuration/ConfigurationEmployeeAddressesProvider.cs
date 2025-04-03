using Employee_Population_Calculator.Services.API;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Employee_Population_Calculator.Services.Configuration
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
