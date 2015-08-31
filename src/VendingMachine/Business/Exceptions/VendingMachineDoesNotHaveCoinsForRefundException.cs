using System;
using System.Diagnostics.Contracts;

namespace VendingMachineApp.Business.Exceptions
{
    public sealed class VendingMachineDoesNotHaveCoinsForRefundException : Exception
    {
        public readonly Wallet MachineWallet;
        public readonly decimal RefundAmount;

        public VendingMachineDoesNotHaveCoinsForRefundException(Wallet machineWallet, decimal refundAmount)
            :base(string.Format("Vending Machine does not have coins for refund. Wallet total: {0}. Refund amount: {1}", machineWallet.TotalFunds(), refundAmount))
        {
            Contract.Requires(machineWallet != null);
            Contract.Requires(refundAmount > decimal.Zero);

            MachineWallet = machineWallet;
            RefundAmount = refundAmount;
        }
    }
}
