using System.Collections.Generic;
using System.Diagnostics.Contracts;
using VendingMachineApp.Business;

namespace VendingMachineApp.Commands
{
    public sealed class BuyCommand
    {
        public readonly IReadOnlyCollection<Coin> Deposit;
        public readonly GoodsIdentity Goods;

        public BuyCommand(IReadOnlyCollection<Coin> deposit, GoodsIdentity goods)
        {
            Contract.Requires(deposit != null);
            Contract.Requires(!goods.Equals(default(GoodsIdentity)));

            Deposit = deposit;
            Goods = goods;
        }
    }
}
