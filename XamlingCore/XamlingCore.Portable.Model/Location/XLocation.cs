namespace XamlingCore.Portable.DTO.Location
{
    public class XLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
        public XPositionStatus Status { get; set; }
        public bool IsResolved { get; set; }
        public bool IsEnabled { get; set; }
    }
}
