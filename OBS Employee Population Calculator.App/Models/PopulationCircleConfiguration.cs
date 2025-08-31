namespace OBS_Employee_Population_Calculator.App.Models
{
    /// <summary>
    /// Repräsentiert die Konfigurationsdaten, die für die Berechnung
    /// und spätere Darstellung eines PopulationCircle verwendet werden.
    /// Diese Klasse dient typischerweise als **Zwischenschritt**:
    /// - Sie enthält die Daten, wie ein Kreis berechnet oder gestaltet werden soll.
    /// - Die Informationen (Farbe, Tooltip-Inhalt, Prozentanteil) werden dann genutzt,
    ///   um ein <see cref="PopulationCircle"/>-Objekt zu erstellen, das direkt
    ///   auf der Karte dargestellt wird.
    ///
    /// Dadurch wird die Logik von *Datenquelle* (z. B. Mitarbeiterstatistik)
    /// und *Visualisierung* (Leaflet.js, OpenStreetMap) sauber getrennt.
    /// </summary>
    public class PopulationCircleConfiguration
    {
        public string ColorHex { get; set; }

        public string TooltipContents { get; set; }

        public double PercentageOfEmployees { get; set; }
    }
}
