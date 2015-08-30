﻿using System.Collections;
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
            _coins = coins
                .GroupBy(c => c.ParValue)
                .Select(c => c.Aggregate(0, (i, coin) => i + coin.Count, count => new Coin(c.Key, count)))
                .ToImmutableHashSet(new CoinEqualityComparer());
        }

        public Wallet Put(IReadOnlyCollection<Coin> coins)
        {
            Contract.Requires(coins != null);

            return coins.Aggregate(this, (current, coin) => current.Put(coin));
        }

        public Wallet Put(Coin coin)
        {
            if (coin.Total == decimal.Zero)
            {
                return this;
            }

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

            return coins.Aggregate(this, (current, coin) => current.Retrieve(coin));
        }

        public Wallet Retrieve(Coin coin)
        {
            if (coin.Total == decimal.Zero)
            {
                return this;
            }

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
                .Remove(retrievedCoins);

            if (retrievedCoins.Count != coin.Count)
            {
                retrievedCoins = retrievedCoins.Substract(coin.Count);
                updatedWallet = updatedWallet.Add(retrievedCoins);
            }

            return new Wallet(updatedWallet);
        }

        [Pure]
        public decimal TotalFunds()
        {
            return _coins.Aggregate(decimal.Zero, (current, next) => current + next.ParValue * next.Count);
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
