using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace VendingMachineApp.Utils
{
    public static class CollectionExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            Contract.Requires(source != null);
            Contract.Ensures(Contract.Result<HashSet<T>>() != null);

            return new HashSet<T>(source);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            Contract.Requires(source != null);
            Contract.Requires(comparer != null);
            Contract.Ensures(Contract.Result<HashSet<T>>() != null);

            return new HashSet<T>(source, comparer);
        }
    }
}
