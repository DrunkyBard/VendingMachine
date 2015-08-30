using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace VendingMachine.Utils
{
    public sealed class EqualityComparerFactory
    {
        private class AnonymousEqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _equals;
            private readonly Func<T, int> _getHashCode; 

            internal AnonymousEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
            {
                _equals = equals;
                _getHashCode = getHashCode;
            }

            public bool Equals(T x, T y)
            {
                return _equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return _getHashCode(obj);
            }
        }

        public static IEqualityComparer<T> Create<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            Contract.Requires(equals != null);
            Contract.Requires(getHashCode != null);
            Contract.Ensures(Contract.Result<IEqualityComparer<T>>() != null);

            return new AnonymousEqualityComparer<T>(equals, getHashCode);
        }
    }
}