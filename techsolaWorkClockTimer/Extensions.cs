using System;
using System.Collections.Generic;

namespace techsolaWorkClockTimer
{
    internal static class Extensions
    {
        public static TimeSpan Sum<T>(this IEnumerable<T> source, Func<T, TimeSpan> selector)
        {
            var total = TimeSpan.Zero;

            foreach (var value in source)
                total += selector(value);

            return total;
        }
    }
}
