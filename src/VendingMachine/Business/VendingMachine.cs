using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using VendingMachine.Business.Events;

namespace VendingMachine.Business
{
    public class VendingMachine
    {
        private Wallet _vendingMachineWallet;
        private Wallet _buyerWallet;

        public VendingMachine(Wallet vendingMachineWallet, Wallet buyerWallet)
        {
            Contract.Requires(vendingMachineWallet != null);
            Contract.Requires(buyerWallet != null);

            _vendingMachineWallet = vendingMachineWallet;
            _buyerWallet = buyerWallet;
        }

        public FundsRecoveredEvent RecoverFunds(IReadOnlyCollection<Coin> funds)
        {
            Contract.Requires(funds != null);
            Contract.Ensures(Contract.Result<FundsRecoveredEvent>() != null);

            //var groupedFunds = funds.Aggregate(decimal.Zero, ())

            return new FundsRecoveredEvent(_vendingMachineWallet, _buyerWallet);
        }

        public void BuyGoods(decimal funds, Goods goods)
        {
        }
    }
}