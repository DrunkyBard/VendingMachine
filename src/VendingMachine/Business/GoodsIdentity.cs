using System;
using System.Diagnostics.Contracts;

namespace VendingMachineApp.Business
{
    public struct GoodsIdentity : IEquatable<GoodsIdentity>
    {
        public readonly Guid Identity;

        public GoodsIdentity(Guid identity)
        {
            Contract.Requires(identity != default(Guid));

            Identity = identity;
        }

        public override string ToString()
        {
            return Identity.ToString();
        }

        public bool Equals(GoodsIdentity other)
        {
            return Identity == other.Identity;
        }
    }
}