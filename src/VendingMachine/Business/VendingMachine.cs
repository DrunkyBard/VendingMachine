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

        public FundsRecoveredEvent RecoverFunds(IReadOnlyCollection<Coin> depositedAmount)
        {
            Contract.Requires(depositedAmount != null);
            Contract.Requires(depositedAmount.Any());
            Contract.Ensures(Contract.Result<FundsRecoveredEvent>() != null);

            var total = depositedAmount.Aggregate(decimal.Zero, (current, coin) => current + coin.Total);
            _vendingMachineWallet = _vendingMachineWallet.Put(depositedAmount);
            _buyerWallet = _buyerWallet.Retrieve(depositedAmount);

            var orderedWallet = _vendingMachineWallet.OrderByDescending(x => x.ParValue);
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

                    if (recoveredCoinCount == 0)
                    {
                        continue;
                    }

                    total -= coin.ParValue * recoveredCoinCount;
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