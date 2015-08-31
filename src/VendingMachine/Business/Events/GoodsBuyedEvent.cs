using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace VendingMachineApp.Business.Events
{
    public sealed class GoodsBuyedEvent
    {
        public readonly Wallet UpdatedMachineWallet;
        public readonly Wallet UpdatedBuyerWallet;
        public readonly Goods BuyedGoods;
        public readonly IReadOnlyCollection<Coin> Refunds; 

        public GoodsBuyedEvent(
            Wallet vendingMachineWallet, 
            Wallet buyerWallet, 
            Goods buyedGoods,
            IReadOnlyCollection<Coin> refunds)
        {
            Contract.Requires(vendingMachineWallet != null && vendingMachineWallet.ShowCoins().Any());
            Contract.Requires(buyerWallet != null);
            Contract.Requires(refunds != null);
            Contract.Requires(refunds.Count == 0 || refunds.Any() && buyerWallet.ShowCoins().Any());

            UpdatedMachineWallet = vendingMachineWallet;
            UpdatedBuyerWallet = buyerWallet;
            BuyedGoods = buyedGoods;
            Refunds = refunds;
        }
    }
}
