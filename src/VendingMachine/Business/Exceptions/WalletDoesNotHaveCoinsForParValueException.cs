using System;
using System.Diagnostics.Contracts;

namespace VendingMachine.Business.Exceptions
{
    public sealed class WalletDoesNotHaveCoinsForParValueException : Exception
    {
        public readonly int ParValue;

        public WalletDoesNotHaveCoinsForParValueException(int parValue) 
            : base(string.Format("Wallet does not have coins with par value {0}", parValue))
        {
            Contract.Requires(parValue > 0);

            ParValue = parValue;
        }
    }
}
