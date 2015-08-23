using System;
using System.Linq;
using VendingMachine.Business;
using VendingMachine.Business.Exceptions;
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
            Assert.True(walletCoins.Single().Equals(coin));
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

            Assert.True(walletCoins.Single().Equals(inWalletCoin));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 10)]
        [InlineData(2, 1)]
        [InlineData(2, 10)]
        [InlineData(5, 1)]
        [InlineData(5, 10)]
        public void WhenRetrieveCoinFromWallet_AndWalletHaveFundsForGivenPar_ThenWalletShouldDeleteGivenCoin(int parValue, int coinCount)
        {
            var coin = new Coin(parValue, coinCount);
            var inWalletCoinCount = new Random().Next(coinCount, coinCount + 100);
            var inWalletCoin = new Coin(parValue, inWalletCoinCount);
            var wallet = new CoinWallet(new [] {inWalletCoin});

            var walletCoins = wallet
                .Retrieve(coin)
                .ShowCoins();
            inWalletCoin = inWalletCoin.Substract(coinCount);

            Assert.True(walletCoins.Single().Equals(inWalletCoin));
        }

        [Theory]
        [InlineData(1, 5, 7)]
        [InlineData(1, 10, 2)]
        [InlineData(2, 5, 5)]
        [InlineData(2, 10, 1)]
        [InlineData(5, 5, 2)]
        [InlineData(5, 10, 10)]
        public void Retrieve_WhenWalletDoesNotHaveCoinsForGivenPar_ThenShouldThrowDoesNotHaveCoinException(int parValue, int coinCount, int inWalletCoinPar)
        {
            var coin = new Coin(parValue, coinCount);
            var inWalletCoinCount = new Random().Next(0, coinCount - 5);
            var inWalletCoin = new Coin(inWalletCoinPar, inWalletCoinCount);
            var wallet = new CoinWallet(new [] {inWalletCoin});

            Assert.Throws<WalletDoesNotHaveCoinsForParValueException>(() => wallet.Retrieve(coin));
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(1, 10)]
        [InlineData(2, 5)]
        [InlineData(2, 10)]
        [InlineData(5, 5)]
        [InlineData(5, 10)]
        public void Retrieve_WhenInsufficientFundsInWalletForGivenPar_ThenShouldThrowDoesNotHaveFundsException(int parValue, int coinCount)
        {
            var coin = new Coin(parValue, coinCount);
            var inWalletCoinCount = new Random().Next(0, coinCount - 5);
            var inWalletCoin = new Coin(parValue, inWalletCoinCount);
            var wallet = new CoinWallet(new [] {inWalletCoin});

            Assert.Throws<InsufficientFundsWithParValueException>(() => wallet.Retrieve(coin));
        }
    }
}
