using System.Collections.Generic;

namespace OBS_Employee_Population_Calculator.App.Services.Configuration
{
    /// <summary>
    /// Diese Klasse dient als Data Transfer Object (DTO) und wird aus appsettings.json gebunden.
    /// Sie Enthält die notwendigen Zugangsdaten, um den OAuth2/OpenID-Flow zu realisieren.
    /// </summary>
    public class AuthenticationConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public List<string> Scopes { get; set; }
    }
}
