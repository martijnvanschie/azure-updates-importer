using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Updates.Importer.Cli.Core
{
    internal class DateTimeUtils
    {
        public static int GetWeekNumber(DateTime date)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            Calendar calendar = culture.Calendar;

            int weekNumber = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }

        public static string ToUniversalDateTimeTimetString(DateTime date)
        {
            string formattedDate = date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            return formattedDate;
        }

        public static DateTime FromUnversalDateTimeString(string unversalDateTimeString)
        {
            return DateTime.ParseExact(unversalDateTimeString, "R", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }
    }
}
