using System;

namespace Api.Rest.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetCurrnetAge(this DateTimeOffset dateTimeOffset)
        {
            if (dateTimeOffset == null)
                throw new ArgumentNullException(nameof(dateTimeOffset));
            var currentDate = DateTime.UtcNow;
            int age = currentDate.Year - dateTimeOffset.Year;
            return age;
        }
    }
}