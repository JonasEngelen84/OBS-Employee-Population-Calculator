using Geolocation;

namespace OBS_Employee_Population_Calculator.App.Services.API
{
    /// <summary>
    /// SOLID-Relevanz:
    ///  - SRP: Das Interface stellt lediglich Unternehmensinformationen bereit.
    ///  - OCP: Später können unterschiedliche Implementierungen hinzugefügt werden. Die Implementierungen, die das Interface konsumiert, bleibt unverändert.
    ///  - LSP: Jede Implementierung des Interfaces kann überall dort eingesetzt werden, wo die Erwartungen des Clients nicht verletzt werden.
    ///  - ISP: Das Interface ist spezifisch und stellt nur die benötigten Methoden bereit.
    ///  - DIP: Abstraktionen (Interfaces) werden bevorzugt gegenüber konkreten Implementierungen.
    /// </summary>
    public interface ICompanyInformationProvider
    {   
        Coordinate CompanyCoordinate { get; }
        string CompanyAddress {  get; }
    }
}
