using System;
using IndoorPositioning.Models;

namespace IndoorPositioning.Services
{
    public class Triangulate
    {
        public static Location GetLocationWithCenterOfGravity(Location beaconA, Location beaconB, Location beaconC, double distanceA, double distanceB, double distanceC)
        {
            //Every meter there are approx 4.5 points
            double METERS_IN_COORDINATE_UNITS_RATIO = 4.5;

            //http://stackoverflow.com/a/524770/663941
            //Find Center of Gravity
            double cogX = (beaconA.Latitude + beaconB.Latitude + beaconC.Latitude) / 3;
            double cogY = (beaconA.Longitude + beaconB.Longitude + beaconC.Longitude) / 3;
            Location cog = new Location { Latitude = cogX, Longitude = cogY};


            //Nearest Beacon
            Location nearestBeacon;
            double shortestDistanceInMeters;
            if (distanceA < distanceB && distanceA < distanceC)
            {
                nearestBeacon = beaconA;
                shortestDistanceInMeters = distanceA;
            }
            else if (distanceB < distanceC)
            {
                nearestBeacon = beaconB;
                shortestDistanceInMeters = distanceB;
            }
            else
            {
                nearestBeacon = beaconC;
                shortestDistanceInMeters = distanceC;
            }

            //http://www.mathplanet.com/education/algebra-2/conic-sections/distance-between-two-points-and-the-midpoint
            //Distance between nearest beacon and COG
            double distanceToCog = Math.Sqrt(Math.Pow(cog.Latitude - nearestBeacon.Latitude, 2)
                    + Math.Pow(cog.Longitude - nearestBeacon.Longitude, 2));

            //Convert shortest distance in meters into coordinates units.
            double shortestDistanceInCoordinationUnits = shortestDistanceInMeters * METERS_IN_COORDINATE_UNITS_RATIO;

            //http://math.stackexchange.com/questions/46527/coordinates-of-point-on-a-line-defined-by-two-other-points-with-a-known-distance?rq=1
            //On the line between Nearest Beacon and COG find shortestDistance point apart from Nearest Beacon

            double t = shortestDistanceInCoordinationUnits / distanceToCog;

            Location pointsDiff = new Location
            {
                Latitude = cog.Latitude - nearestBeacon.Latitude,
                Longitude = cog.Longitude - nearestBeacon.Longitude
            };

            Location tTimesDiff = new Location {
                Latitude = pointsDiff.Latitude * t,
                Longitude = pointsDiff.Longitude * t
            };

            //Add t times diff with nearestBeacon to find coordinates at a distance from nearest beacon in line to COG.

            Location estLocation = new Location
            {
                Latitude = nearestBeacon.Latitude + tTimesDiff.Latitude,
                Longitude = nearestBeacon.Longitude + tTimesDiff.Longitude
            };
            return estLocation;
        }
    }
}
