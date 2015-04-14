using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using XamlingCore.Portable.Contract.Device.Location;
using XamlingCore.Portable.Model.Location;

namespace XamlingCore.Windows8.Implementations
{
    public class MotionSensor : IMotionSensor
    {
        public XMotion CurrentMotion
        {
            get { return _get(); }
        }

        XMotion _get()
        {
            if (_inclinometer != null)
            {
                var reading = _inclinometer.GetCurrentReading();

                return new XMotion
                {
                    Pitch = reading.PitchDegrees,
                    Roll = reading.RollDegrees,
                    Yaw = reading.YawDegrees
                };
                
            }

            return null;
        }

        private Inclinometer _inclinometer;

        public void Start()
        {
            _inclinometer = Inclinometer.GetDefault();
        }

        public void Stop()
        {
            _inclinometer = null;
        }
    }
}
