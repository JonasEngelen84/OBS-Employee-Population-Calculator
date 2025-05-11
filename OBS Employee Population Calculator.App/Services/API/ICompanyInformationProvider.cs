using Geolocation;

namespace OBS_Employee_Population_Calculator.App.Services.API
{
    public interface ICompanyInformationProvider
    {   
        Coordinate CompanyCoordinate { get; }
        string CompanyAddress {  get; }
    }
}
