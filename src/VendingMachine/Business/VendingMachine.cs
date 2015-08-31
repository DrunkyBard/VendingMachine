using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using VendingMachineApp.Business.Events;
using VendingMachineApp.Business.Exceptions;

namespace VendingMachineApp.Business
{
    public sealed class VendingMachine
    {
        private Wallet _vendingMachineWallet;
        private Wallet _buyerWallet;
        private readonly BagOfGoods _availableGoods;

        public VendingMachine(Wallet vendingMachineWallet, Wallet buyerWallet, IList<Goods> availableGoods)
        {
            Contract.Requires(vendingMachineWallet != null);
            Contract.Requires(buyerWallet != null);
            Contract.Requires(availableGoods != null);

            _vendingMachineWallet = vendingMachineWallet;
            _buyerWallet = buyerWallet;
            _availableGoods = new BagOfGoods(availableGoods);
        }

        public CoinsRefundedEvent Refund(IReadOnlyCollection<Coin> depositedAmount)
        {
            Contract.Requires(depositedAmount != null);
            Contract.Requires(depositedAmount.Any());
            Contract.Ensures(Contract.Result<CoinsRefundedEvent>() != null);

            var total = depositedAmount.Aggregate(decimal.Zero, (current, coin) => current + coin.Total);
            _vendingMachineWallet = _vendingMachineWallet.Put(depositedAmount);
            _buyerWallet = _buyerWallet.Retrieve(depositedAmount);

            var refundCoins = Refund(total);

            _vendingMachineWallet = _vendingMachineWallet.Retrieve(refundCoins);
            _buyerWallet = _buyerWallet.Put(refundCoins);

            return new CoinsRefundedEvent(_vendingMachineWallet, _buyerWallet);
        }

        public GoodsBuyedEvent BuyGoods(IReadOnlyCollection<Coin> depositedAmount, GoodsIdentity goods)
        {
            Contract.Requires(depositedAmount != null);

            var availableGoods = _availableGoods.Retrieve(goods);
            var depositWallet = new Wallet(depositedAmount);
            var depositTotal = depositWallet.TotalFunds();

            if (depositTotal < availableGoods.Price)
            {
                throw new InsufficientAmountForBuyingGoodsException(availableGoods.Price, depositTotal, goods);
            }

            var refundAmount = depositTotal - availableGoods.Price;
            _vendingMachineWallet = _vendingMachineWallet.Put(depositedAmount);
            _buyerWallet = _buyerWallet.Retrieve(depositedAmount);

            var refundCoins = Refund(refundAmount);
            var refundWallet = new Wallet(refundCoins);

            if (refundWallet.TotalFunds() != refundAmount)
            {
                throw new VendingMachineDoesNotHaveCoinsForRefundException(_vendingMachineWallet, refundAmount);
            }

            _vendingMachineWallet = _vendingMachineWallet.Retrieve(refundCoins);
            _buyerWallet = _buyerWallet.Put(refundCoins);

            return new GoodsBuyedEvent(_vendingMachineWallet, _buyerWallet, availableGoods, refundCoins);
        }

        private IReadOnlyCollection<Coin> Refund(decimal amount)
        {
            var orderedMachineWallet = _vendingMachineWallet.OrderByDescending(x => x.ParValue);
            var refundCoins = new List<Coin>();

            foreach (var coin in orderedMachineWallet)
            {
                if (coin.Total < amount)
                {
                    amount -= coin.Total;
                    refundCoins.Add(coin);
                }
                else
                {
                    var recoveredCoinCount = (int)(amount / coin.ParValue);

                    if (recoveredCoinCount == 0)
                    {
                        continue;
                    }

                    amount -= coin.ParValue * recoveredCoinCount;
                    refundCoins.Add(new Coin(coin.ParValue, recoveredCoinCount));
                }
            }

            return refundCoins;
        }
    }
}