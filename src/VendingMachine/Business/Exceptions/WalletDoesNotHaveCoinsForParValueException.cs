using System;
using System.Diagnostics.Contracts;

namespace VendingMachineApp.Business.Exceptions
{
    public sealed class WalletDoesNotHaveCoinsForParValueException : Exception
    {
        public readonly decimal ParValue;

        public WalletDoesNotHaveCoinsForParValueException(decimal parValue) 
            : base(string.Format("Wallet does not have coins with par value {0}", parValue))
        {
            Contract.Requires(parValue > 0);

            ParValue = parValue;
        }
    }
}
