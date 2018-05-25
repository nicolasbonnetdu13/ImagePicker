﻿namespace ImagePicker.iOS.Helpers
{
	using System;
	using Foundation;

	public static class DateHelper
	{
		public static DateTime ToDateTime(this NSDate date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2001, 1, 1, 0, 0, 0));
			return reference.AddSeconds(date.SecondsSinceReferenceDate);
		}

		public static NSDate ToNSDate(this DateTime date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2001, 1, 1, 0, 0, 0));
			return NSDate.FromTimeIntervalSinceReferenceDate((date - reference).TotalSeconds);
		}
	}
}
