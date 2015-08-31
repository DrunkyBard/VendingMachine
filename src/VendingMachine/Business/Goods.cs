using System;
using System.Diagnostics.Contracts;

namespace VendingMachineApp.Business
{
    public sealed class Goods : IEquatable<Goods>
    {
        public readonly GoodsIdentity Identity;
        public readonly decimal Price;
        public readonly int Count;

        public Goods(GoodsIdentity identity, decimal price, int count)
        {
            Contract.Requires(price > decimal.Zero);
            Contract.Requires(count >= 0);

            Identity = identity;
            Price = price;
            Count = count;
        }

        public bool Equals(Goods other)
        {
            return
                Identity.Equals(other.Identity) &&
                Price == other.Price &&
                Count == other.Count;
        }
    }
}
