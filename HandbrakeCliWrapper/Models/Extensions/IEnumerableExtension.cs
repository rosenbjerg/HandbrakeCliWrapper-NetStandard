using System;
using System.Collections.Generic;

namespace HandbrakeCliWrapper.Models.Extensions {
    internal static class IEnumerableExtension {
        internal static bool AllOrButOne<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {
            int notCount = 0;
            foreach (TSource item in source) {
                if (!predicate(item))
                    notCount++;

                if (notCount > 1)
                    return false;
            }

            return true;
        }
    }
}