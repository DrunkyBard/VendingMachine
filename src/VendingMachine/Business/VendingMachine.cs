using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using VendingMachine.Business.Events;

namespace VendingMachine.Business
{
    public sealed class VendingMachine
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

            _vendingMachineWallet = _vendingMachineWallet.Put(funds);
            var orderedWallet = _vendingMachineWallet.OrderByDescending(x => x.ParValue);
            var total = funds.Aggregate(decimal.Zero, (currentFund, nextCoint) => currentFund + nextCoint.ParValue * nextCoint.Count);
            var recoveredCoins = new List<Coin>();

            foreach (var coin in orderedWallet)
            {
                if (coin.Total < total)
                {
                    total -= coin.Total;
                    recoveredCoins.Add(coin);
                }
                else
                {
                    var recoveredCoinCount = (int)(total / coin.ParValue);
                    total -= coin.ParValue*recoveredCoinCount;
                    recoveredCoins.Add(new Coin(coin.ParValue, recoveredCoinCount));
                }
            }

            _vendingMachineWallet = _vendingMachineWallet.Retrieve(recoveredCoins);
            _buyerWallet = _buyerWallet.Put(recoveredCoins);

            return new FundsRecoveredEvent(_vendingMachineWallet, _buyerWallet);
        }

        public void BuyGoods(decimal funds, Goods goods)
        {
        }
    }
}