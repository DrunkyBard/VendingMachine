using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using VendingMachine.Business;
using Xunit;

namespace VendingMachine.Tests
{
    public sealed class VendingMachineTests
    {
        [Fact]
        public void RecoverFunds_WhenBuyerWantsRecoverZeroAmount_ThenVendingMachineShouldPreventRecoverOperation()
        {
            var machineWallet = new Wallet();
            var buyerWallet = new Wallet();
            var vendingMachine = new Business.VendingMachine(machineWallet, buyerWallet);
            Contract.ContractFailed += (sender, args) =>
            {
                Assert.True(args.FailureKind == ContractFailureKind.Precondition);
                args.SetHandled();
            };

            vendingMachine.RecoverFunds(Enumerable.Empty<Coin>().ToList());
        }

        [Theory]
        [MemberData("RecoverDepositedAmountFixture", MemberType = typeof(VendingMachineTestFixture))]
        public void RecoverFunds_WhenBuyerWantsRecoverDepositedAmount_ThenVendingMachineShouldRecoverFundsWithLessCoins(
            Wallet machineWallet,
            Wallet buyerWallet,
            IReadOnlyCollection<Coin> depositedAmount,
            Wallet expectedMachineWallet,
            Wallet expectedBuyerWallet)
        {
            var vendingMachine = new Business.VendingMachine(machineWallet, buyerWallet);

            var @event = vendingMachine.RecoverFunds(depositedAmount);
            var recoveredMachineWallet = @event.VendingMachineWallet;
            var recoveredBuyerWallet = @event.BuyerWallet;

            if (!expectedMachineWallet.Any())
            {
                Assert.True(!recoveredMachineWallet.Any());
            }
            else
            {
                var expectedBuyerWalletMatch = !expectedBuyerWallet
                    .Except(recoveredBuyerWallet)
                    .Any();

                Assert.True(expectedBuyerWalletMatch);
            }

            var expectedMachineWalletMatch = !expectedMachineWallet
                .Except(recoveredMachineWallet)
                .Any();

            Assert.True(expectedMachineWalletMatch);
        }
    }
}
