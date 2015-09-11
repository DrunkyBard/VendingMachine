using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using VendingMachineApp.Business;
using VendingMachineApp.Business.Exceptions;
using Xunit;

namespace VendingMachineApp.Tests
{
    public sealed class VendingMachineTests
    {
        [Fact]
        public void Refund_WhenBuyerWantsRefundZeroAmount_ThenVendingMachineShouldPreventRefundOperation()
        {
            var machineWallet = new Wallet();
            var buyerWallet = new Wallet();
            var vendingMachine = CreateMachine(machineWallet, buyerWallet);
            Contract.ContractFailed += (sender, args) =>
            {
                Assert.True(args.FailureKind == ContractFailureKind.Precondition);
                args.SetHandled();
            };

            vendingMachine.Refund(Enumerable.Empty<Coin>().ToList());
        }

        [Theory]
        [MemberData("RecoverDepositedAmountFixture", MemberType = typeof(VendingMachineTestFixture))]
        public void Refund_WhenBuyerWantsRefundDepositedAmount_ThenVendingMachineShouldRefundWithLessCoins(
            Wallet machineWallet,
            Wallet buyerWallet,
            IReadOnlyCollection<Coin> depositedAmount,
            Wallet expectedMachineWallet,
            Wallet expectedBuyerWallet)
        {
            var vendingMachine = CreateMachine(machineWallet, buyerWallet);

            var @event = vendingMachine.Refund(depositedAmount);
            var refundedMachineWallet = @event.VendingMachineWallet;
            var refundedBuyerWallet = @event.BuyerWallet;

            if (!expectedMachineWallet.Any())
            {
                Assert.True(!refundedMachineWallet.Any());
            }
            else
            {
                var expectedBuyerWalletMatch = !expectedBuyerWallet
                    .Except(refundedBuyerWallet)
                    .Any();

                Assert.True(expectedBuyerWalletMatch);
            }

            var expectedMachineWalletMatch = !expectedMachineWallet
                .Except(refundedMachineWallet)
                .Any();

            Assert.True(expectedMachineWalletMatch);
        }

        [Theory]
        [MemberData("BuyGoodsWithSufficientAmountFixture", MemberType = typeof(VendingMachineTestFixture))]
        public void WhenBuyGoodsWithSufficientAmount_ThenVendingMachineShouldSellOnlyOneGoods(
            Wallet givenMachineWallet,
            Wallet givenBuyerWallet,
            IList<Goods> availableGoods,
            IReadOnlyCollection<Coin> depositedAmount,
            Goods buyedGoods,
            Wallet expectedMachineWallet,
            Wallet expectedBuyerWallet,
            IReadOnlyCollection<Coin> refund)
        {
            var vendingMachine = CreateMachine(givenMachineWallet, givenBuyerWallet, availableGoods);

            var @event = vendingMachine.BuyGoods(depositedAmount, buyedGoods.Identity);

            Assert.True(buyedGoods.Count - @event.BuyedGoods.Count == 1);
        }

        [Theory]
        [MemberData("BuyGoodsWithSufficientAmountFixture", MemberType = typeof(VendingMachineTestFixture))]
        public void WhenBuyGoodsWithSufficientAmount_AndDepositGreaterThanGoodsPrice_ThenVendingMachineGiveBuyerRefunds(
            Wallet givenMachineWallet,
            Wallet givenBuyerWallet,
            IList<Goods> availableGoods,
            IReadOnlyCollection<Coin> depositedAmount,
            Goods buyedGoods,
            Wallet expectedMachineWallet,
            Wallet expectedBuyerWallet,
            IReadOnlyCollection<Coin> refund)
        {
            var vendingMachine = CreateMachine(givenMachineWallet, givenBuyerWallet, availableGoods);

            var @event = vendingMachine.BuyGoods(depositedAmount, buyedGoods.Identity);
            var updatedMachineWallet = @event.UpdatedMachineWallet
                .ShowCoins()
                .OrderBy(x => x.ParValue);
            var updatedBuyerWallet = @event.UpdatedBuyerWallet
                .ShowCoins()
                .OrderBy(x => x.ParValue);
            var buyerRefund = @event.Refunds;

            Assert.Equal(refund.OrderBy(x => x.ParValue), buyerRefund.OrderBy(x => x.ParValue));
            Assert.Equal(expectedMachineWallet.ShowCoins().OrderBy(x => x.ParValue), updatedMachineWallet);
            Assert.Equal(expectedBuyerWallet.ShowCoins().OrderBy(x => x.ParValue), updatedBuyerWallet);
        }

        [Theory]
        [MemberData("BuyGoodsWithInsufficientAmountFixture", MemberType = typeof(VendingMachineTestFixture))]
        public void WhenBuyGoodsWithInsufficientAmount_AndGoodsAvailableInMachine_ThenVendingMachineShouldThrowInsufficientAmountException(
            Wallet givenMachineWallet,
            Wallet givenBuyerWallet,
            IList<Goods> availableGoods,
            IReadOnlyCollection<Coin> depositedAmount,
            Goods buyedGoods)
        {
            var vendingMachine = CreateMachine(givenMachineWallet, givenBuyerWallet, availableGoods);
            
            var ex = Assert.Throws<InsufficientAmountForBuyingGoodsException>(() => vendingMachine.BuyGoods(depositedAmount, buyedGoods.Identity));
            var depositedWallet = new Wallet(depositedAmount);

            Assert.True(ex.Goods.Equals(buyedGoods.Identity));
            Assert.True(ex.ActualFunds == depositedWallet.TotalFunds());
            Assert.True(ex.ExpectedFunds == buyedGoods.Price);
        }

        [Theory]
        [MemberData("VendingMachineDoesntHaveGoodsFixture", MemberType = typeof(VendingMachineTestFixture))]
        public void WhenBuyGoods_AndGoodsNotAvailableInMachine_ThenMachineShouldThrowGoodsShortageException(
            Wallet givenMachineWallet,
            Wallet givenBuyerWallet,
            IList<Goods> availableGoods,
            IReadOnlyCollection<Coin> depositedAmount,
            Goods buyedGoods)
        {
            var vendingMachine = CreateMachine(givenMachineWallet, givenBuyerWallet, availableGoods);

            var ex = Assert.Throws<GoodsShortageException>(() => vendingMachine.BuyGoods(depositedAmount, buyedGoods.Identity));

            Assert.True(ex.Goods.Equals(buyedGoods));
            Assert.True(ex.ExpectedCount == 1);
        }

        [Theory]
        [MemberData("BuyGoodsWithSufficientAmountAndVendingMachineCannotRefundFixture", MemberType = typeof(VendingMachineTestFixture))]
        public void WhenBuyGoodsWithSufficientAmount_AndMachineCannotRefund_ThenVendingMachineShouldThrowVendingMachineDoesNotHaveCoinsForRefundException(
            Wallet givenBuyerWallet,
            Wallet givenMachineWallet,
            IList<Goods> availableGoods,
            IReadOnlyCollection<Coin> depositedAmount,
            Goods buyedGoods,
            Wallet expectedMachineWalletAfterDeposit,
            decimal expectedRefund)
        {
            var vendingMachine = CreateMachine(givenMachineWallet, givenBuyerWallet, availableGoods);

            var ex = Assert.Throws<VendingMachineDoesNotHaveCoinsForRefundException>(() => vendingMachine.BuyGoods(depositedAmount, buyedGoods.Identity));

            Assert.Equal(ex.MachineWallet.ShowCoins().OrderByDescending(x => x.ParValue), expectedMachineWalletAfterDeposit.ShowCoins().OrderByDescending(x => x.ParValue));
            Assert.True(ex.RefundAmount == expectedRefund);
        }

        private VendingMachine CreateMachine(Wallet machineWallet, Wallet buyerWallet, IList<Goods> goods = null)
        {
            if (goods == null)
            {
                goods = Enumerable.Empty<Goods>().ToList();
            }

            return new VendingMachine(machineWallet, buyerWallet, goods);
        }
    }
}
