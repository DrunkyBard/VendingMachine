using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using VendingMachine.Business.Exceptions;

namespace VendingMachine.Business
{
    public class Wallet : IEnumerable<Coin>
    {
        private readonly ImmutableHashSet<Coin> _coins;

        public Wallet()
        {
            _coins = ImmutableHashSet.Create<Coin>(new CoinEqualityComparer());
        }

        public Wallet(IEnumerable<Coin> coins)
        {
            _coins = coins.ToImmutableHashSet(new CoinEqualityComparer());
        }

        public Wallet Put(IReadOnlyCollection<Coin> coins)
        {
            Contract.Requires(coins != null);
            
            Wallet wallet = this;

            foreach (var coin in coins)
            {
                wallet = wallet.Put(coin);
            }

            return wallet;
        }

        public Wallet Put(Coin coin)
        {
            Coin oldValue;
            var newCoinValue = _coins.TryGetValue(coin, out oldValue) 
                ? oldValue.Add(coin.Count) 
                : coin;
            var updatedWallet = _coins
                .Remove(oldValue)
                .Add(newCoinValue);
            
            return new Wallet(updatedWallet);
        }

        public Wallet Retrieve(IReadOnlyCollection<Coin> coins)
        {
            Contract.Requires(coins != null);

            Wallet wallet = this;

            foreach (var coin in coins)
            {
                wallet = wallet.Retrieve(coin);
            }

            return wallet;
        }

        public Wallet Retrieve(Coin coin)
        {
            Coin retrievedCoins;

            if (!_coins.TryGetValue(coin, out retrievedCoins))
            {
                throw new WalletDoesNotHaveCoinsForParValueException(coin.ParValue);
            }

            if (retrievedCoins.Count < coin.Count)
            {
                throw new InsufficientFundsWithParValueException(coin);
            }

            var updatedWallet = _coins
                .Remove(retrievedCoins)
                .Add(retrievedCoins.Substract(coin.Count));

            return new Wallet(updatedWallet);
        }

        public decimal TotalFunds()
        {
            return _coins.Aggregate(decimal.Zero, (current, next) => current + next.ParValue*next.Count);
        }

        [Pure]
        public IReadOnlyCollection<Coin> ShowCoins()
        {
            return _coins.ToList();
        }

        public IEnumerator<Coin> GetEnumerator()
        {
            return _coins.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        private class CoinEqualityComparer : IEqualityComparer<Coin>
        {
            public bool Equals(Coin x, Coin y)
            {
                return x.ParValue == y.ParValue;
            }

            public int GetHashCode(Coin obj)
            {
                return obj.ParValue.GetHashCode();
            }
        }
    }
}
