using System;
using System.Linq;
using VendingMachine.Business;
using Xunit;

namespace VendingMachine.Tests
{
    public sealed class CoinWalletTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 10)]
        [InlineData(2, 1)]
        [InlineData(2, 10)]
        [InlineData(5, 1)]
        [InlineData(5, 10)]
        public void WhenPutCoinInEmptyWallet_ThenCoinShouldBeInGivenWallet(int parValue, int coinCount)
        {
            var coin = new Coin(parValue, coinCount);
            var wallet = new CoinWallet();

            var walletCoins = wallet
                .Put(coin)
                .ShowCoins();

            Assert.True(walletCoins.Count == 1);
            Assert.True(walletCoins.Single() == coin);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 10)]
        [InlineData(2, 1)]
        [InlineData(2, 10)]
        [InlineData(5, 1)]
        [InlineData(5, 10)]
        public void WhenPutCoinInWalletWithGivenParValue_ThenWalletShouldContainSumForThisParValue(int parValue, int coinCount)
        {
            var coin = new Coin(parValue, coinCount);
            var inWalletCoinCount = new Random().Next(0, 100);
            var inWalletCoin = new Coin(parValue, inWalletCoinCount);
            var wallet = new CoinWallet(new [] { inWalletCoin });

            var walletCoins = wallet
                .Put(coin)
                .ShowCoins();
            inWalletCoin = inWalletCoin.Add(coinCount);

            Assert.True(walletCoins.Single() == inWalletCoin);
        }
    }
}
