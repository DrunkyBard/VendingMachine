using System;
using System.Diagnostics.Contracts;

namespace VendingMachine.Business.Exceptions
{
    public sealed class WalletDoesNotHaveCoinsWithParValueException : Exception
    {
        public readonly int ParValue;

        public WalletDoesNotHaveCoinsWithParValueException(int parValue) 
            : base(string.Format("Wallet does not have coins with par value {0}", parValue))
        {
            Contract.Requires(parValue > 0);

            ParValue = parValue;
        }
    }
}
