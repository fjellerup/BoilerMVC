using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoilerMVC
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> TakePage<T>(this IEnumerable<T> items, int page, int pageSize = 10) where T : class
        {
            return items.Skip(pageSize * (page - 1)).Take(pageSize);
        }
    }
}