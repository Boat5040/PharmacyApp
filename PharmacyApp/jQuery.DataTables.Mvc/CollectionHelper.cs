using jQuery.DataTables.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyApp.jQuery.DataTables.Mvc
{
    public static class CollectionHelper
    {
        public static IOrderedEnumerable<TSource> CustomSort<TSource, TKey>(this IEnumerable<TSource> items, SortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == SortingDirection.Ascending)
            {
                return items.OrderBy(keySelector);
            }

            return items.OrderByDescending(keySelector);
        }

        public static IOrderedEnumerable<TSource> CustomSort<TSource, TKey>(this IOrderedEnumerable<TSource> items, SortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == SortingDirection.Ascending)
            {
                return items.ThenBy(keySelector);
            }

            return items.ThenByDescending(keySelector);
        }

    }
}