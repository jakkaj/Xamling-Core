namespace XamlingCore.Portable.Model.Location
{
    public class XLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
        public XPositionStatus Status { get; set; }
        public bool IsResolved { get; set; }
        public bool IsEnabled { get; set; }
        public double? Heading { get; set; }
        public bool? HeadingAvailable { get; set; }
        public double? Altitude { get; set; }
        public double? AltitudeAccuracy { get; set; }
        public double? Speed { get; set; }

        public string Name { get; set; }
    }
}
