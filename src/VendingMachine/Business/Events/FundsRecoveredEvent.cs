using System.Diagnostics.Contracts;

namespace VendingMachine.Business.Events
{
    public sealed class FundsRecoveredEvent
    {
        public readonly Wallet VendingMachineWallet;
        public readonly Wallet BuyerWallet;

        public FundsRecoveredEvent(Wallet machineWallet, Wallet buyerWallet)
        {
            Contract.Requires(machineWallet != null);
            Contract.Requires(buyerWallet != null && buyerWallet.ShowCoins().Count > 0);

            VendingMachineWallet = machineWallet;
            BuyerWallet = buyerWallet;
        }
    }
}
