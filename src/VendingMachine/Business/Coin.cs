using System;
using System.Diagnostics.Contracts;

namespace VendingMachine.Business
{
    public struct Coin : IEquatable<Coin>
    {
        public readonly decimal ParValue;
        public readonly int Count;

        public Coin(decimal parValue, int count)
        {
            Contract.Requires(parValue > 0);
            Contract.Requires(count >= 0);

            ParValue = parValue;
            Count = count;
        }

        public Coin Add(int coins)
        {
            Contract.Requires(coins > 0);

            return new Coin(ParValue, Count + coins);
        }

        public Coin Substract(int coins)
        {
            Contract.Requires(Count >= coins);

            return new Coin(ParValue, Count - coins);
        }

        public decimal Total {
            get { return ParValue*Count; }
        }

        public bool Equals(Coin other)
        {
            return ParValue == other.ParValue && Count == other.Count;
        }
    }
}