using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace XamlingCore.iOS.Implementations.Location
{
    // This is a direct port of the Xamarin.Mobile class 
    // https://github.com/xamarin/Xamarin.Mobile/tree/master/MonoTouch/Xamarin.Mobile/Geolocation

    internal class GeolocationSingleUpdateDelegate
        : CLLocationManagerDelegate
    {
        public GeolocationSingleUpdateDelegate(CLLocationManager manager, double desiredAccuracy, bool includeHeading, int timeout, CancellationToken cancelToken)
        {
            this.manager = manager;
            this.tcs = new TaskCompletionSource<Position>(manager);
            this.desiredAccuracy = desiredAccuracy;
            this.includeHeading = includeHeading;

            if (timeout != Timeout.Infinite)
            {
                Timer t = null;
                t = new Timer(s =>
                {
                    if (this.haveLocation)
                        this.tcs.TrySetResult(new Position(this.position));
                    else
                        this.tcs.TrySetCanceled();

                    StopListening();
                    t.Dispose();
                }, null, timeout, 0);
            }

            cancelToken.Register(() =>
            {
                StopListening();
                this.tcs.TrySetCanceled();
            });
        }

        public Task<Position> Task
        {
            get { return this.tcs.Task; }
        }

        public override void AuthorizationChanged(CLLocationManager manager, CLAuthorizationStatus status)
        {
            // If user has services disabled, we're just going to throw an exception for consistency.
            if (status == CLAuthorizationStatus.Denied || status == CLAuthorizationStatus.Restricted)
            {
                StopListening();
                this.tcs.TrySetException(new GeolocationException(GeolocationError.Unauthorized));
            }
        }

        public override void Failed(CLLocationManager manager, MonoTouch.Foundation.NSError error)
        {
            switch ((CLError)error.Code)
            {
                case CLError.Network:
                    StopListening();
                    this.tcs.SetException(new GeolocationException(GeolocationError.PositionUnavailable));
                    break;
            }
        }

        public override bool ShouldDisplayHeadingCalibration(CLLocationManager manager)
        {
            return true;
        }

        public override void UpdatedLocation(CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
        {
            if (newLocation.HorizontalAccuracy < 0)
                return;

            if (this.haveLocation && newLocation.HorizontalAccuracy > this.position.Accuracy)
                return;

            this.position.Accuracy = newLocation.HorizontalAccuracy;
            this.position.Altitude = newLocation.Altitude;
            this.position.AltitudeAccuracy = newLocation.VerticalAccuracy;
            this.position.Latitude = newLocation.Coordinate.Latitude;
            this.position.Longitude = newLocation.Coordinate.Longitude;
            this.position.Speed = newLocation.Speed;
            this.position.Timestamp = new DateTimeOffset(newLocation.Timestamp);

            this.haveLocation = true;

            if ((!this.includeHeading || this.haveHeading) && this.position.Accuracy <= this.desiredAccuracy)
            {
                this.tcs.TrySetResult(new Position(this.position));
                StopListening();
            }
        }

        public override void UpdatedHeading(CLLocationManager manager, CLHeading newHeading)
        {
            if (newHeading.HeadingAccuracy < 0)
                return;
            if (this.bestHeading != null && newHeading.HeadingAccuracy >= this.bestHeading.HeadingAccuracy)
                return;

            this.bestHeading = newHeading;
            this.position.Heading = newHeading.TrueHeading;
            this.haveHeading = true;

            if (this.haveLocation && this.position.Accuracy <= this.desiredAccuracy)
            {
                this.tcs.TrySetResult(new Position(this.position));
                StopListening();
            }
        }

        private bool haveHeading;
        private bool haveLocation;
        private readonly Position position = new Position();
        private CLHeading bestHeading;

        private readonly double desiredAccuracy;
        private readonly bool includeHeading;
        private readonly TaskCompletionSource<Position> tcs;
        private readonly CLLocationManager manager;

        private void StopListening()
        {
            if (CLLocationManager.HeadingAvailable)
                this.manager.StopUpdatingHeading();

            this.manager.StopUpdatingLocation();
        }
    }
}