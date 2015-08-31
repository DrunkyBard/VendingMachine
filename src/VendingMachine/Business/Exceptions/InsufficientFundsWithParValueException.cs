using System;

namespace VendingMachineApp.Business.Exceptions
{
    public sealed class InsufficientFundsWithParValueException : Exception
    {
        public readonly Coin InsufficientFunds;

        public InsufficientFundsWithParValueException(Coin insufficientFunds)
            : base(string.Format("Wallet does not have {0} coins with par value {1}", insufficientFunds.Count, insufficientFunds.ParValue))
        {
            InsufficientFunds = insufficientFunds;
        }
    }
}
