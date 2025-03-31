using Geolocation;

namespace OBS.Dashboard.Map.Services.API
{
    public interface ICompanyInformationProvider
    {   
        Coordinate CompanyCoordinate { get; }
        string CompanyAddress {  get; }
    }
}
