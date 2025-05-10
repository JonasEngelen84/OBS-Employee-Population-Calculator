using Geolocation;

namespace Employee_Population_Calculator.App.Services.API
{
    public interface ICompanyInformationProvider
    {   
        Coordinate CompanyCoordinate { get; }
        string CompanyAddress {  get; }
    }
}
