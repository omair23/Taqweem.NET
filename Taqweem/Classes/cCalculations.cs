using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taqweem.Classes
{
    public class cCalculations
    {
        public static double GetTimeZoneDifference(string TimeZoneId, DateTime DateVal)
        {
            try
            {
                TimeZoneInfo TZ = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);

                var Diff = TZ.GetUtcOffset(DateVal);

                return TimeSpanToDouble(Diff);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static double TimeSpanToDouble(TimeSpan Val)
        {
            int Hour = Val.Hours;

            double Minute = (double)Val.Minutes / (double)60;

            while (Minute >= 1)
            {
                Hour += 1;
                Minute = (Minute - 1);
            }

            return Hour + Minute;
        }

        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2)
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;

            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;

            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);

            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            return dist * 1.609344;
        }

        public static double DegreeBearing(double lat1, double long1, double lat2, double long2)
        {
            double a = lat1 * Math.PI / 180;
            double b = long1 * Math.PI / 180;
            double c = lat2 * Math.PI / 180;
            double d = long2 * Math.PI / 180;

            if (Math.Cos(c) * Math.Sin(d - b) == 0)
                if (c > a)
                    return 0;
                else
                    return 180;
            else
            {
                double angle = Math.Atan2(Math.Cos(c) * Math.Sin(d - b), Math.Sin(c) * Math.Cos(a) - Math.Sin(a) * Math.Cos(c) * Math.Cos(d - b));
                return (angle * 180 / Math.PI + 360) % 360;

            }
        }

        //public static double DegreeBearing(
        //    double lat1, double lon1,
        //    double lat2, double lon2)
        //{
        //    var dLon = ToRad(lon2 - lon1);
        //    var dPhi = Math.Log(
        //        Math.Tan(ToRad(lat2) / 2 + Math.PI / 4) / Math.Tan(ToRad(lat1) / 2 + Math.PI / 4));
        //    if (Math.Abs(dLon) > Math.PI)
        //        dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
        //    return ToBearing(Math.Atan2(dLon, dPhi));
        //}

        public static double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }

        public static double ToBearing(double radians)
        {
            // convert radians to degrees (as bearing: 0...360)
            return (ToDegrees(radians) + 360) % 360;
        }
    }
}
