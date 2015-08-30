using System.Collections.Generic;
using VendingMachine.Business;

namespace VendingMachine.Tests
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
    }
}
