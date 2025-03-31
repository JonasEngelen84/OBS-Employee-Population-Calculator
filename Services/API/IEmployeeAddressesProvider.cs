using System.Collections.Generic;

namespace OBS.Dashboard.Map.Services.API
{
    public interface IEmployeeAddressesProvider
    {
        List<string> EmployeesAddresses { get; }
    }
}
