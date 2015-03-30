using Android.App;
using Android.Hardware;
using System;
using XamlingCore.Portable.Contract.Device.Location;
using XamlingCore.Portable.Model.Location;
using Android.Runtime;

namespace XamlingCore.Droid.Implementations
{
    public class MotionSensor : Java.Lang.Object, IMotionSensor, ISensorEventListener
    {
        //https://github.com/eziosoft/MultiWii_EZ_GUI/blob/master/src/com/ezio/multiwii/helpers/Sensors.java

        private SensorManager _sensorManager;
        private static readonly object _syncLock = new object();

        SensorManager m_sensorManager;
        float[] m_lastMagFields = new float[3];
        float[] m_lastAccels = new float[3];
        private float[] m_rotationMatrix = new float[16];
        private float[] m_orientation = new float[4];

        public float Pitch = 0f;
        public float Heading = 0f;
        public float Roll = 0f;


        public event EventHandler MotionUpdated;

        public XMotion CurrentMotion { get; private set; }

        public void Start()
        {
            _sensorManager = (SensorManager)Application.Context.GetSystemService(Android.App.Activity.SensorService);
            _sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor(SensorType.MagneticField), SensorDelay.Normal);
            _sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Normal);            
        }


        public void Stop()
        {
            if (_sensorManager == null)
            {
                return;
            }

            _sensorManager.UnregisterListener(this);
            _sensorManager = null;
        }



        public void OnSensorChanged(SensorEvent e)
        {
            switch (e.Sensor.Type) {                
                case SensorType.Accelerometer:
                    e.Values.CopyTo(m_lastAccels, 0);
                    break;
                case SensorType.MagneticField:
                    e.Values.CopyTo(m_lastMagFields, 0);
                    break;
                default:
                    return;
            }

            computeOrientation();
        }

        private void computeOrientation()
        {
            if (SensorManager.GetRotationMatrix(m_rotationMatrix, null, m_lastAccels, m_lastMagFields))
            {
                SensorManager.GetOrientation(m_rotationMatrix, m_orientation);

                float yaw = (float)(RadianToDegree(m_orientation[0])); /* + Declination (Based on phone's location?) */
                float pitch = (float)RadianToDegree(m_orientation[1]);
                float roll = (float)RadianToDegree(m_orientation[2]);

                var xMotion = new XMotion
                {
                    Pitch = pitch,
                    Roll = roll,
                    Yaw = yaw
                };

                //Console.WriteLine("Pitch: " + xMotion.Pitch);
                //Console.WriteLine("Roll: " + xMotion.Roll);
                //Console.WriteLine("Yaw: " + xMotion.Yaw + " ----");

                //TODO: Low pass filtering? Evens out results. -- http://www.raweng.com/blog/2013/05/28/applying-low-pass-filter-to-android-sensors-readings/
                //var xMotion = new XMotion
                //{
                //    Pitch = lowPass(pitch),
                //    Roll = lowPass(roll),
                //    Yaw = lowPass(yaw)
                //};

                CurrentMotion = xMotion;

                if (MotionUpdated != null)
                {
                    MotionUpdated(this, EventArgs.Empty);
                }
            }
        }



        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum]SensorStatus accuracy)
        {
            /* Do nothing */
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
