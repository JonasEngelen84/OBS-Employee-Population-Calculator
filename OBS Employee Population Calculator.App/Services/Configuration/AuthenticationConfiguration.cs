using System.Collections.Generic;

namespace OBS_Employee_Population_Calculator.App.Services.Configuration
{
    // Klasse zur Bereitstellung der benötigten Informationen für OAuth 2
    public class AuthenticationConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public List<string> Scopes { get; set; }
    }
}
