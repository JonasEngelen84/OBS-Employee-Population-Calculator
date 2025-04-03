using Geolocation;

namespace Employee_Population_Calculator.Services.API
{
    public interface ICompanyInformationProvider
    {   
        Coordinate CompanyCoordinate { get; }
        string CompanyAddress {  get; }
    }
}
