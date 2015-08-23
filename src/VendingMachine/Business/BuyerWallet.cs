using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace VendingMachine.Business
{
    public sealed class BuyerWallet
    {
        private readonly List<Coin> _balance;

        public BuyerWallet() : this(new Coin[0])
        {
        }

        public BuyerWallet(Coin[] balance)
        {
            Contract.Requires(balance != null);

            _balance = balance.ToList();
        }
    }
}
