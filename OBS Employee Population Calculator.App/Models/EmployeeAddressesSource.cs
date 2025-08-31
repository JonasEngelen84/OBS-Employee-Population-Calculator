namespace OBS_Employee_Population_Calculator.App.Models
{
    /// <summary>
    /// Steuert die Quelle, aus der Geokoordinaten für Mitarbeiter ermittelt werden.
    /// Dieser Optionsschalter wird über die Anwendungskonfiguration (z. B. appsettings.json)
    /// an CoordinateSource gebunden und entscheidet, welche Implementierung von
    /// IEmployeeCoordinatesProvider via Dependency Injection registriert wird.
    /// Best Practice: Enums vermeiden Tippfehler in Konfigurationswerten,
    /// machen Intent explizit und erleichtern spätere Erweiterungen (z. B. weitere Geocoding-Provider).
    /// </summary>
    public enum EmployeeAddressesSource
    {
        Configuration, //appsettings
        OBSStamm
    }
}
