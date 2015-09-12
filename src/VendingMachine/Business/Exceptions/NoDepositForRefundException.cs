using System;

namespace VendingMachineApp.Business.Exceptions
{
    public class NoDepositForRefundException : Exception
    {
        public NoDepositForRefundException() : base("Buyer must make deposits before refund operation")
        {}
    }
}