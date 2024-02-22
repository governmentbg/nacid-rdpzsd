using System;
using System.Collections.Generic;

namespace Rdpzsd.Models.Extensions
{
    public static class CollectionDuplicateValuesExtension
    {
        public static bool HasDuplicate<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            var checkItems = new HashSet<T>();

            foreach (var item in collection)
            {
                if (checkItems.Add(item))
                {
                    continue;
                }

                return true;
            }

            return false;
        }
    }
}
