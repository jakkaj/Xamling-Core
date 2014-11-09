using System;
using System.Collections.Generic;
using System.Text;
using MonoTouch.CoreMotion;
using MonoTouch.Foundation;
using XamlingCore.Portable.Contract.Device.Location;
using XamlingCore.Portable.Model.Location;

namespace XamlingCore.iOS.Implementations
{
    public class MotionSensor : IMotionSensor
    {
        private CMMotionManager _motionManager;

        public event EventHandler MotionUpdated;

        public XMotion CurrentMotion { get; private set; }

        public void Start()
        {
            _motionManager = new CMMotionManager();
            _motionManager.StartDeviceMotionUpdates(NSOperationQueue.CurrentQueue, (data, error) =>
            {
                var xMotion = new XMotion
                {
                    Pitch = data.Attitude.Pitch,
                    Roll = data.Attitude.Roll,
                    Yaw = data.Attitude.Yaw
                };
                
                CurrentMotion = xMotion;
                
                if (MotionUpdated != null)
                {
                    MotionUpdated(this, EventArgs.Empty);
                }
            });
        }

        public void Stop()
        {
            if (_motionManager == null)
            {
                return;
            }
            _motionManager.StopDeviceMotionUpdates();
            _motionManager = null;
        }
    }
}
