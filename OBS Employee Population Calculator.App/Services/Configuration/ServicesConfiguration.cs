namespace OBS_Employee_Population_Calculator.App.Services.Configuration
{
    /// <summary>
    /// Diese Klasse ist als Plain Old CLR Object (POCO) implementiert und wird direkt aus appsettings befüllt.
    /// Sie repräsentiert die Konfigurationseinstellungen für externe Services, auf die die Anwendung zugreift.
    /// Vorteil: Die Service-URLs sind zentral konfigurierbar und nicht im Code hardcodiert.
    /// </summary>
    public class ServicesConfiguration
    {
        public string ServiceTemplateUrl { get; set; }
        public string StammServiceUrl { get; set; }
        public string STSServiceUrl { get; set; }
    }
}
