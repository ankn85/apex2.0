using System;

namespace Apex.Core.Extensions
{
    public static class DateTimeExtensions
    {
        private const string DatePattern = "MM/dd/yyyy";
        //private const string DateTimePattern = "MM/dd/yyyy HH:mm:ss";
        private const string DateTimePattern = "MM/dd/yyyy hh:mm:ss tt";

        public static DateTime StartOfDay(this DateTime datetime)
        {
            return datetime.Date;
        }

        public static DateTime EndOfDay(this DateTime datetime)
        {
            return datetime.Date.AddDays(1).AddTicks(-1);
        }

        public static bool Between(this DateTime datetime, DateTime start, DateTime end)
        {
            long ticks = datetime.Ticks;
            return start.Ticks <= ticks && ticks <= end.Ticks;
        }

        public static string ToDateString(this DateTime datetime)
        {
            return datetime.ToString(DatePattern);
        }

        public static string ToDateTimeString(this DateTime datetime)
        {
            return datetime.ToString(DateTimePattern);
        }

        public static string ToPrettyDate(this DateTime datetime)
        {
            TimeSpan timeSpan = DateTime.UtcNow.Subtract(datetime);
            int totalDays = (int)timeSpan.TotalDays;

            if (totalDays == 0)
            {
                int totalSeconds = (int)timeSpan.TotalSeconds;

                if (totalSeconds < 60)
                {
                    return "Just now";
                }

                if (totalSeconds < 120)
                {
                    return "1 minute ago";
                }

                if (totalSeconds < 3600)
                {
                    return $"{Math.Floor(totalSeconds / 60.0)} minutes ago";
                }

                if (totalSeconds < 7200)
                {
                    return "1 hour ago";
                }

                if (totalSeconds < 86400)
                {
                    return $"{Math.Floor(totalSeconds / 3600.0)} hours ago";
                }
            }
            else if (totalDays == 1)
            {
                return "Yesterday";
            }
            else if (totalDays < 31)
            {
                return $"{totalDays} days ago";
            }
            else if (totalDays < 365)
            {
                int totalMonths = (int)Math.Ceiling(totalDays / 30.0);

                if (totalMonths == 13)
                {
                    totalMonths--;
                }

                return $"{totalMonths} months ago";
            }

            return $"{Math.Ceiling(totalDays / 364.0)} years ago";
        }
    }
}