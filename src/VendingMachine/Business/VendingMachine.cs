using System.Diagnostics.Contracts;
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

        public FundsRecoveredEvent RecoverFunds(decimal funds)
        {
            Contract.Requires(funds != decimal.Zero);
            Contract.Ensures(Contract.Result<FundsRecoveredEvent>() != null);

            return new FundsRecoveredEvent(_vendingMachineWallet, _buyerWallet);
        }

        public void BuyGoods(decimal funds, Goods goods)
        {
        }
    }
}