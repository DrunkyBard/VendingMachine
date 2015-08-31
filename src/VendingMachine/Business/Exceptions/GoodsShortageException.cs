using System;
using System.Diagnostics.Contracts;

namespace VendingMachineApp.Business.Exceptions
{
    public sealed class GoodsShortageException : Exception
    {
        public readonly Goods Goods;
        public readonly int ExpectedCount;

        public GoodsShortageException(Goods goods, int expectedCount)
        {
            Contract.Requires(expectedCount > goods.Count);

            Goods = goods;
            ExpectedCount = expectedCount;
        }
    }
}
