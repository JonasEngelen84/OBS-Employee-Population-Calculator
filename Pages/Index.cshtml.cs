using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OBS.Dashboard.Map.Models;
using OBS.Dashboard.Map.Services.API;

//C# basierter Code mit Bezug auf den HTML Code
namespace OBS.Dashboard.Map.Pages
{
    //Beeinhaltet die Daten für die Index-Seite
    public partial class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly List<PopulationCircle> circles;
        private readonly string companyLat;
        private readonly string companyLng;
        private readonly string companyAddress;

        public IndexModel(ILogger<IndexModel> logger, ICompanyInformationProvider companyInformationProvider, ICirclesInformationProvider circlesInformationProvider)
        {
            _logger = logger;
            circles = circlesInformationProvider.Circles;
            companyLat = companyInformationProvider.CompanyCoordinate.Latitude.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"));
            companyLng = companyInformationProvider.CompanyCoordinate.Longitude.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"));
            companyAddress = companyInformationProvider.CompanyAddress;
        }

        public string CompanyLat => companyLat;

        public string CompanyLng => companyLng;

        public string CompanyAddress => companyAddress;

        public List<PopulationCircle> Circles => circles;
    }
}
