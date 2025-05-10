using System.Collections.Generic;

namespace Employee_Population_Calculator.App.Services.API
{
    public interface IEmployeeAddressesProvider
    {
        List<string> EmployeesAddresses { get; }
    }
}
