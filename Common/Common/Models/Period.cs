using System;
using Guards;

namespace KUtil.Common.Models
{
	public struct Period : IEquatable<Period>
	{
		public Period(DateTimeOffset startDate, DateTimeOffset endDate)
		{
			Guard.CheckTrue(
				endDate >= startDate,
				() => new ArgumentException("End date must be >= start date."));

			StartDate = startDate;
			EndDate = endDate;
		}

		public bool IsIn(DateTimeOffset date)
		{
			return date >= StartDate && date <= EndDate;
		}

		public override bool Equals(object obj)
		{
            Guard.CheckNotNull(obj, nameof(obj));

			return Equals((Period)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (StartDate.GetHashCode() * 397) ^ EndDate.GetHashCode();
			}
		}

		public bool Equals(Period other)
		{
			return StartDate == other.StartDate && EndDate == other.EndDate;
		}

		public static bool operator==(Period a, Period b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Period a, Period b)
		{
			return !(a == b);
		}

		public override string ToString()
		{
			return $"{StartDate:O} — {EndDate:O}";
		}

		public DateTimeOffset StartDate { get; }

		public DateTimeOffset EndDate { get; }
	}
}
