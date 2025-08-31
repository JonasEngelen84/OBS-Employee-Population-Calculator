namespace OBS_Employee_Population_Calculator.App.Models
{
    /// <summary>
    /// Steuert die Quelle, aus der Mitarbeiteradressen bezogen werden.
    /// Dieser Optionsschalter wird über die Anwendungskonfiguration (z. B. <c>appsettings.json</c>)
    /// an EmployeeAddressesSource gebunden und steuert zur Laufzeit,
    /// welche Implementierung von IEmployeeAddressesProvider via Dependency Injection registriert wird.
    /// Best Practice: Enums vermeiden Tippfehler in Konfigurationswerten,
    /// machen Intent explizit und erleichtern spätere Erweiterungen (z. B. weitere Geocoding-Provider).
    /// </summary>
    public enum CoordinateSource
    {
        Configuration, //appsettings
        Nominatim
    }
}
