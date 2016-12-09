using System;

namespace Obsequy.Utility
{
    public static class DateTimeHelper
    {
        public static DateTime Now
        {
			get
			{
				// return a UTC now
				return DateTime.UtcNow;
			}
        }

        public static DateTime Today
        {
            get
            {
                // return a date that is today
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
        }

        public static DateTime Tomorrow
        {
            get
            {
                // return a date that is tomorrow
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            }
        }

		public static string FileNameNow
		{
			get
			{
				// return a UTC now suitable for file names
				return string.Format("{0:yyyy.MM.dd-HH.mm.ss}", DateTime.UtcNow);
			}
		}

		public static DateTime? Empty
		{
			// return an empty DateTime
			get 
			{
				return null;
			}
		}

		public static DateTime? ClipAsDateTime(DateTime? dateTime)
		{
			if (dateTime != null)
				return new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day, dateTime.Value.Hour, dateTime.Value.Minute, dateTime.Value.Second);
			return null;
		}

        public static DateTime? ClipAsDate(DateTime? dateTime)
        {
            if (dateTime != null)
                return new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day);
            return null;
        }

		public static bool HasTimeElapsed(DateTime? dateTime)
		{
			if (dateTime != null)
			{
				// has the specified number of hours elapsed since this value was set?
				return (dateTime.Value <= Now);
			}

			return false;
		}

        public static bool IsDateEqual(DateTime? date1, DateTime? date2)
        {
            // are the date (year, month, day) equal?
            if (!date1.HasValue && !date2.HasValue)
                return true;
            
            if (date1.HasValue && date2.HasValue)
            {
                if (date1.Value.Year == date2.Value.Year &&
                    date1.Value.Month == date2.Value.Month &&
                    date1.Value.Day == date2.Value.Day)
                    return true;
            }

            return false;
        }

		public static bool IsUtcDateEqualToLocalDate(DateTime? utc, DateTime? local)
		{
			var utc1 = ClipAsDateTime(utc);
			var utc2 = ToUniversalTime(local);

			if (utc1 == utc2)
				return true;

			return false;
		}

		public static DateTime? ToUniversalTime(DateTime? local)
		{
			if (local != null)
				return new DateTime(local.Value.Year, local.Value.Month, local.Value.Day, local.Value.Hour, local.Value.Minute, local.Value.Second).ToUniversalTime();
			return null;
		}

		public static DateTime? ToLocalTime(DateTime? utc)
		{
			if (utc != null)
				return utc.Value.ToLocalTime();
			return null;
		}
    }
}
