using XamlingCore.Portable.Model.Location;

namespace XamlingCore.Portable.Contract.Device.Location
{
    public interface IMotionSensor
    {
        XMotion CurrentMotion { get; }
        void Start();
        void Stop();
    }
}