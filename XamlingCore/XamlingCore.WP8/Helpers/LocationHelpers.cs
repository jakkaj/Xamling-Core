using System;
using System.Device.Location;
using Windows.Devices.Geolocation;
using AutoMapper;
using XamlingCore.Portable.Model.Location;

namespace XamlingCore.WP8.Helpers
{
    public class LocationHelpers
    {
        public static GeoCoordinate ConvertGeocoordinate(Geocoordinate geocoordinate)
        {
            return new GeoCoordinate
                (
                geocoordinate.Latitude,
                geocoordinate.Longitude,
                geocoordinate.Altitude ?? Double.NaN,
                geocoordinate.Accuracy,
                geocoordinate.AltitudeAccuracy ?? Double.NaN,
                geocoordinate.Speed ?? Double.NaN,
                geocoordinate.Heading ?? Double.NaN
                );
        }

        public static XLocation ConvertXLocation(Geocoordinate geocoordinate, XLocation existing)
        {
            return Mapper.Map(geocoordinate, existing);
        }

        public static XPositionStatus ConvertLocationStatus(PositionStatus status)
        {
            return Mapper.Map<XPositionStatus>(status);
        }

        public static double Distance(double lat, double lng, XLocation b)
        {
            var aCord = new GeoCoordinate(lat, lng);
            var bCord = new GeoCoordinate(b.Latitude, b.Longitude);

            return aCord.GetDistanceTo(bCord);
        }
    }
}
