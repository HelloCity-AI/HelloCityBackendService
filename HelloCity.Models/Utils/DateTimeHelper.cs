using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloCity.Models.Utils
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Get the current UTC Time.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetUtcNow() => DateTime.UtcNow;

        /// <summary>
        /// Get the current time in Sydney (AUS Eastern Standard Time).
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSydneyNow() => ConvertUtcToSydney(DateTime.UtcNow);


        /// <summary>
        /// Converts a UTC DateTime to Sydney local time， cross-platform compatible.
        /// </summary>
        /// <param name="utc"></param>
        /// <returns></returns>
        public static DateTime ConvertUtcToSydney(DateTime utc)
        {
            TimeZoneInfo sydneyTimeZone;
            try
            {
                //win
                sydneyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                //linux / mac
                sydneyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Australia/Sydney");
            }
            catch (InvalidTimeZoneException)
            {
                return utc;
            }

            var sydneyTime = TimeZoneInfo.ConvertTimeFromUtc(utc, sydneyTimeZone);

            // 👇 Force DateTimeKind.Utc (required for PostgreSQL timestamp with time zone)
            return DateTime.SpecifyKind(sydneyTime, DateTimeKind.Utc);
        }
    }
}
