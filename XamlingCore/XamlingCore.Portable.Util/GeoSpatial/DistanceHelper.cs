//From: http://stackoverflow.com/questions/28569246/how-to-get-distance-between-two-locations-in-windows-phone-8-1


using System;
using XamlingCore.Portable.Model.Location;

namespace XamlingCore.Portable.Util.GeoSpatial  
{  
/// <summary>  
/// The distance type to return the results in.  
/// </summary>  
public enum DistanceType { Miles, Kilometers };  
/// <summary>  
/// Specifies a Latitude / Longitude point.  
/// </summary>  
public struct Position  
{  
    public double Latitude;  
    public double Longitude;  
}

    public static class DistanceHelper
    {
        /// <summary>  
        /// Returns the distance in miles or kilometers of any two  
        /// latitude / longitude points.  
        /// </summary>  
        public static double DistanceBetween(XLocation pos1, XLocation pos2, DistanceType type)
        {
            double R = (type == DistanceType.Miles) ? 3960 : 6371;
            double dLat = _toRadian(pos2.Latitude - pos1.Latitude);
            double dLon = _toRadian(pos2.Longitude - pos1.Longitude);
            double a = Math.Sin(dLat/2)*Math.Sin(dLat/2) +
                       Math.Cos(_toRadian(pos1.Latitude))*Math.Cos(_toRadian(pos2.Latitude))*
                       Math.Sin(dLon/2)*Math.Sin(dLon/2);
            double c = 2*Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R*c;
            return d;
        }

        /// <summary>  
        /// Convert to Radians.  
        /// </summary>  
        private static double _toRadian(double val)
        {
            return (Math.PI/180)*val;
        }
    }
}