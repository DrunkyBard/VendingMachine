using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachineApp.Business;

namespace VendingMachineApp.Tests
{
    public sealed class VendingMachineTestFixture
    {
        public static IEnumerable<object[]> RecoverDepositedAmountFixture()
        {
            //23
            yield return new object[]
            {
                new Wallet(),                                                           // Given Machine wallet
                new Wallet(new[] {new Coin(10, 2), new Coin(2, 1), new Coin(1, 1)}),    // Given Buyer wallet
                new[] {new Coin(10, 2), new Coin(2, 1), new Coin(1, 1)},                // Recover funds
                new Wallet(),                                                           // Expected Machine wallet
                new Wallet(new[] {new Coin(10, 2), new Coin(2, 1), new Coin(1, 1)})     // Expected Buyer wallet
            };
            //52
            yield return new object[]
            {
                new Wallet(new[] {new Coin(10, 4), new Coin(5, 7), new Coin(2, 3), new Coin(1, 5)}),
                new Wallet(new[] {new Coin(10, 6), new Coin(5, 4), new Coin(2, 12), new Coin(1, 3)}),
                new[] {new Coin(10, 3), new Coin(5, 2), new Coin(2, 5), new Coin(1, 2)},
                new Wallet(new[] {new Coin(10, 2), new Coin(5, 9), new Coin(2, 7), new Coin(1, 7)}),
                new Wallet(new[] {new Coin(10, 8), new Coin(5, 2), new Coin(2, 8), new Coin(1, 1)})
            };
        }

        public static IEnumerable<object[]> BuyGoodsWithSufficientAmountFixture()
        {
            var buyedGoods = new Goods(new GoodsIdentity(Guid.NewGuid()), 35, 15);

            yield return new object[]
            {
                new Wallet(new[] {new Coin(1, 100), new Coin(2, 100), new Coin(5, 100), new Coin(10, 100)}),  // Given Machine wallet
                new Wallet(new[] {new Coin(1, 10), new Coin(2, 30), new Coin(5, 20), new Coin(10, 15)}),      // Given Buyer wallet
                new[]                                                                                         // Available goods
                {
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 13, 10),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 18, 20),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 21, 20),
                    buyedGoods
                },
                new[]                                                                                         // Deposited amount
                {
                    new Coin(1, 8),
                    new Coin(2, 5),
                    new Coin(5, 2),
                    new Coin(10, 1)
                },
                buyedGoods,                                                                                   // Buyed goods 
                new Wallet(new[] {new Coin(1, 107), new Coin(2, 104), new Coin(5, 102), new Coin(10, 101)}),  // Expected Machine wallet
                new Wallet(new[] {new Coin(1, 3), new Coin(2, 26), new Coin(5, 18), new Coin(10, 14)}),       // Expected Buyer wallet
                new[] {new Coin(2, 1), new Coin(1, 1) }                                                       // Refunds
            };

            buyedGoods = new Goods(new GoodsIdentity(Guid.NewGuid()), 21, 20);

            yield return new object[]
            {
                new Wallet(new[] {new Coin(1, 100), new Coin(2, 100), new Coin(5, 100), new Coin(10, 100)}),
                new Wallet(new[] {new Coin(1, 10), new Coin(2, 30), new Coin(5, 20), new Coin(10, 15)}),
                new[]
                {
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 13, 10),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 18, 20),
                    buyedGoods,
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 35, 15)
                },
                new[]
                {
                    new Coin(1, 4),
                    new Coin(2, 5),
                    new Coin(5, 5)
                },
                buyedGoods,
                new Wallet(new[] {new Coin(1, 103), new Coin(2, 104), new Coin(5, 104), new Coin(10, 99)}),
                new Wallet(new[] {new Coin(1, 7), new Coin(2, 26), new Coin(5, 16), new Coin(10, 16)}),
                new[] {new Coin(10, 1), new Coin(5, 1), new Coin(2, 1), new Coin(1, 1)}
            };

            buyedGoods = new Goods(new GoodsIdentity(Guid.NewGuid()), 13, 10);

            yield return new object[]
            {
                new Wallet(new[] {new Coin(1, 100), new Coin(2, 100), new Coin(5, 100), new Coin(10, 100)}),
                new Wallet(new[] {new Coin(1, 10), new Coin(2, 30), new Coin(5, 20), new Coin(10, 15)}),
                new[]
                {
                    buyedGoods,
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 18, 20),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 21, 20),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 35, 15)
                },
                new[]
                {
                    new Coin(1, 4),
                    new Coin(2, 2),
                    new Coin(5, 1)
                },
                buyedGoods,
                new Wallet(new[] {new Coin(1, 104), new Coin(2, 102), new Coin(5, 101), new Coin(10, 100)}),
                new Wallet(new[] {new Coin(1, 6), new Coin(2, 28), new Coin(5, 19), new Coin(10, 15)}),
                new Coin[0]
            };
        }

        public static IEnumerable<object[]> BuyGoodsWithInsufficientAmountFixture()
        {
            var buyedGoods = new Goods(new GoodsIdentity(Guid.NewGuid()), 35, 15);

            yield return new object[]
            {
                new Wallet(new[] {new Coin(1, 100), new Coin(2, 100), new Coin(5, 100), new Coin(10, 100)}),  // Given Machine wallet
                new Wallet(new[] {new Coin(1, 10), new Coin(2, 30), new Coin(5, 20), new Coin(10, 15)}),      // Given Buyer wallet
                new[]                                                                                         // Available goods
                {
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 13, 10),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 18, 20),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 21, 20),
                    buyedGoods
                },
                new[]                                                                                         // Deposited amount
                {
                    new Coin(1, 8),
                    new Coin(2, 5),
                    new Coin(5, 2)
                },
                buyedGoods                                                                                    // Buyed goods 
            };

            buyedGoods = new Goods(new GoodsIdentity(Guid.NewGuid()), 21, 20);

            yield return new object[]
            {
                new Wallet(new[] {new Coin(1, 100), new Coin(2, 100), new Coin(5, 100), new Coin(10, 100)}),
                new Wallet(new[] {new Coin(1, 10), new Coin(2, 30), new Coin(5, 20), new Coin(10, 15)}),
                new[]
                {
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 13, 10),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 18, 20),
                    buyedGoods,
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 35, 15)
                },
                new[]
                {
                    new Coin(1, 4),
                    new Coin(2, 5)
                },
                buyedGoods
            };

            buyedGoods = new Goods(new GoodsIdentity(Guid.NewGuid()), 13, 10);

            yield return new object[]
            {
                new Wallet(new[] {new Coin(1, 100), new Coin(2, 100), new Coin(5, 100), new Coin(10, 100)}),
                new Wallet(new[] {new Coin(1, 10), new Coin(2, 30), new Coin(5, 20), new Coin(10, 15)}),
                new[]
                {
                    buyedGoods,
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 18, 20),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 21, 20),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 35, 15)
                },
                new[]
                {
                    new Coin(1, 4),
                    new Coin(5, 1)
                },
                buyedGoods
            };
        }

        public static IEnumerable<object[]> VendingMachineDoesntHaveGoodsFixture()
        {
            var buyedGoods = new Goods(new GoodsIdentity(Guid.NewGuid()), 35, 0);

            yield return new object[]
            {
                new Wallet(new[] {new Coin(1, 100), new Coin(2, 100), new Coin(5, 100), new Coin(10, 100)}),  // Given Machine wallet
                new Wallet(new[] {new Coin(1, 10), new Coin(2, 30), new Coin(5, 20), new Coin(10, 15)}),      // Given Buyer wallet
                new[]                                                                                         // Available goods
                {
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 13, 10),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 18, 20),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 21, 20),
                    buyedGoods
                },
                new[]                                                                                         // Deposited amount
                {
                    new Coin(1, 8),
                    new Coin(2, 5),
                    new Coin(5, 2)
                },
                buyedGoods                                                                                    // Buyed goods 
            };

            buyedGoods = new Goods(new GoodsIdentity(Guid.NewGuid()), 21, 0);

            yield return new object[]
            {
                new Wallet(new[] {new Coin(1, 100), new Coin(2, 100), new Coin(5, 100), new Coin(10, 100)}),
                new Wallet(new[] {new Coin(1, 10), new Coin(2, 30), new Coin(5, 20), new Coin(10, 15)}),
                new[]
                {
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 13, 10),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 18, 20),
                    buyedGoods,
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 35, 15)
                },
                new[]
                {
                    new Coin(1, 4),
                    new Coin(2, 5)
                },
                buyedGoods
            };

            buyedGoods = new Goods(new GoodsIdentity(Guid.NewGuid()), 13, 0);

            yield return new object[]
            {
                new Wallet(new[] {new Coin(1, 100), new Coin(2, 100), new Coin(5, 100), new Coin(10, 100)}),
                new Wallet(new[] {new Coin(1, 10), new Coin(2, 30), new Coin(5, 20), new Coin(10, 15)}),
                new[]
                {
                    buyedGoods,
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 18, 20),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 21, 20),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 35, 15)
                },
                new[]
                {
                    new Coin(1, 4),
                    new Coin(5, 1)
                },
                buyedGoods
            };
        }

        public static IEnumerable<object[]> BuyGoodsWithSufficientAmountAndVendingMachineCannotRefundFixture()
        {
            var buyedGoods = new Goods(new GoodsIdentity(Guid.NewGuid()), 35, 15);

            yield return new object[]
            {
                new Wallet(new[] {new Coin(1, 100), new Coin(2, 100), new Coin(5, 100), new Coin(10, 100)}),  // Given Buyer wallet
                new Wallet(Enumerable.Empty<Coin>()),                                                         // Given Machine wallet
                new[]                                                                                         // Available goods
                {
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 13, 10),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 18, 20),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 21, 20),
                    buyedGoods
                },
                new[]                                                                                         // Deposited amount
                {
                    new Coin(10, 4)
                },
                buyedGoods,                                                                                   // Buyed goods
                new Wallet(new [] {new Coin(10, 4)}),                                                         // Machine wallet after deposit
                5                                                                                             // Expected refund
            };

            buyedGoods = new Goods(new GoodsIdentity(Guid.NewGuid()), 21, 20);

            yield return new object[]
            {
                new Wallet(new[] {new Coin(1, 100), new Coin(2, 100), new Coin(5, 100), new Coin(10, 100)}),
                new Wallet(Enumerable.Empty<Coin>()),
                new[]
                {
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 13, 10),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 18, 20),
                    buyedGoods,
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 35, 15)
                },
                new[]
                {
                    new Coin(10, 2),
                    new Coin(5, 1)
                },
                buyedGoods,
                new Wallet(new [] { new Coin(10, 2), new Coin(5, 1) }),
                4
            };

            buyedGoods = new Goods(new GoodsIdentity(Guid.NewGuid()), 13, 10);

            yield return new object[]
            {
                new Wallet(new[] {new Coin(1, 100), new Coin(2, 100), new Coin(5, 100), new Coin(10, 100)}),
                new Wallet(Enumerable.Empty<Coin>()),
                new[]
                {
                    buyedGoods,
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 18, 20),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 21, 20),
                    new Goods(new GoodsIdentity(Guid.NewGuid()), 35, 15)
                },
                new[]
                {
                    new Coin(5, 3)
                },
                buyedGoods,
                new Wallet(new[] {new Coin(5, 3)}),
                2
            };
        }
    }
}
