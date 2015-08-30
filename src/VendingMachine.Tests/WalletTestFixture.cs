using System.Collections.Generic;
using VendingMachine.Business;

namespace VendingMachine.Tests
{
    public sealed class WalletTestFixture
    {
        public static IEnumerable<object[]> DistinctCoinsTestFixture()
        {
            yield return new object[]
            {
                new[]
                {
                    new Coin(1, 2), new Coin(1, 4),
                    new Coin(2, 5), new Coin(2, 9)
                },
                new[]
                {
                    new Coin(1, 6), new Coin(2, 14)
                }
            };

            yield return new object[]
            {
                new[]
                {
                    new Coin(10, 2),
                    new Coin(5, 3), new Coin(5, 3),
                    new Coin(1, 2)
                },
                new[]
                {
                    new Coin(10, 2), new Coin(5, 6), new Coin(1, 2)
                }
            };

            yield return new object[]
            {
                new[]
                {
                    new Coin(5, 4), new Coin(5, 7), new Coin(5, 2),
                    new Coin(1, 3)
                },
                new[]
                {
                    new Coin(5, 13), new Coin(1, 3)
                }
            };
        }
    }
}
