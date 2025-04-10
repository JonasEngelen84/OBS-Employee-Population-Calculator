using System.Collections.Generic;

namespace Employee_Population_Calculator.Services.API
{
    public interface IEmployeeAddressesProvider
    {
        List<string> EmployeesAddresses { get; }
    }
}
