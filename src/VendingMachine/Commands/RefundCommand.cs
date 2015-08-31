using System.Collections.Generic;
using System.Diagnostics.Contracts;
using VendingMachineApp.Business;

namespace VendingMachineApp.Commands
{
    public sealed class RefundCommand
    {
        public readonly IReadOnlyCollection<Coin> Deposit; 

        public RefundCommand(IReadOnlyCollection<Coin> deposit)
        {
            Contract.Requires(deposit != null);

            Deposit = deposit;
        }
    }
}