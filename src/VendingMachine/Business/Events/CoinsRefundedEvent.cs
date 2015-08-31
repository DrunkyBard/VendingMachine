using System.Diagnostics.Contracts;

namespace VendingMachineApp.Business.Events
{
    public sealed class CoinsRefundedEvent
    {
        public readonly Wallet VendingMachineWallet;
        public readonly Wallet BuyerWallet;

        public CoinsRefundedEvent(Wallet machineWallet, Wallet buyerWallet)
        {
            Contract.Requires(machineWallet != null);
            Contract.Requires(buyerWallet != null && buyerWallet.ShowCoins().Count > 0);

            VendingMachineWallet = machineWallet;
            BuyerWallet = buyerWallet;
        }
    }
}
