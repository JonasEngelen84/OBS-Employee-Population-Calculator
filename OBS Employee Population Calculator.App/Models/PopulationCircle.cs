namespace OBS_Employee_Population_Calculator.App.Models
{
    /// <summary>
    /// Repräsentiert einen Kreis auf der Karte, der die "Bevölkerungsdichte" visualisiert.
    /// Wird an die Visualisierungsebene im Frontend (Leaflet.js) weitergegeben.
    /// </summary>
    public class PopulationCircle
    {   
        private readonly string color;
        public string Color => color;

        private readonly int radius;
        public int Radius => radius;

        private readonly string popup;
        public string Popup => popup;

        public PopulationCircle(string color, int radius, string popup)
        {
            this.color = color;
            this.radius = radius;
            this.popup = popup;
        }
    }
}
