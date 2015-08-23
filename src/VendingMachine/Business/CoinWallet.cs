using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using VendingMachine.Business.Exceptions;

namespace VendingMachine.Business
{
    public class CoinWallet : IEnumerable<Coin>
    {
        private readonly ImmutableHashSet<Coin> _coins;

        public CoinWallet()
        {
            _coins = ImmutableHashSet.Create<Coin>(new CoinEqualityComparer());
        }

        public CoinWallet(IEnumerable<Coin> coins)
        {
            _coins = coins.ToImmutableHashSet(new CoinEqualityComparer());
        }

        public CoinWallet Put(Coin coin)
        {
            Coin oldValue;
            var newCoinValue = _coins.TryGetValue(coin, out oldValue) 
                ? oldValue.Add(coin.Count) 
                : coin;
            var updatedWallet = _coins
                .Remove(oldValue)
                .Add(newCoinValue);
            
            return new CoinWallet(updatedWallet);
        }

        public CoinWallet Retrieve(Coin coin)
        {
            Coin retrievedCoins;

            if (!_coins.TryGetValue(coin, out retrievedCoins))
            {
                throw new WalletDoesNotHaveCoinsWithParValueException(coin.ParValue);
            }

            if (retrievedCoins.Count < coin.Count)
            {
                throw new InsufficientFundsWithParValueException(coin);
            }

            var updatedWallet = _coins.Add(retrievedCoins.Substract(coin.Count));

            return new CoinWallet(updatedWallet);
        }

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
