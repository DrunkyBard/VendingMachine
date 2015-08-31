using System;
using System.Diagnostics.Contracts;

namespace VendingMachineApp.Business.Exceptions
{
    public sealed class InsufficientAmountForBuyingGoodsException : Exception
    {
        public readonly decimal ExpectedFunds;
        public readonly decimal ActualFunds;
        public readonly GoodsIdentity Goods;

        public InsufficientAmountForBuyingGoodsException(decimal expectedFunds, decimal actualFunds, GoodsIdentity goods)
            : base(string.Format("Insufficient amount for buying goods {0}. Expected funds: {1}, actual funds: {2}", goods, expectedFunds, actualFunds))
        {
            Contract.Requires(expectedFunds > actualFunds);

            ExpectedFunds = expectedFunds;
            ActualFunds = actualFunds;
            Goods = goods;
        }
    }
}
