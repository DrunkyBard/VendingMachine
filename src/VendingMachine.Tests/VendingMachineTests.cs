using System.Collections.Generic;
using System.Linq;
using VendingMachine.Business;
using Xunit;

namespace VendingMachine.Tests
{
    public sealed class VendingMachineTests
    {
        [Fact]
        public void RecoverFunds_WhenBuyerWantsRecoverZeroFunds_ThenVendingMachineShouldPreventRecoverOperation()
        {
            var funds = new List<Coin>
            {
                new Coin(10, 2),
                new Coin(2, 1),
                new Coin(1, 1)
            };
            var buyerWallet = new Wallet(Enumerable.Empty<Coin>());
            var machineWallet = new Wallet(Enumerable.Empty<Coin>());
            var vendingMachine = new Business.VendingMachine(machineWallet, buyerWallet);

            var @event = vendingMachine.RecoverFunds(funds);
            var recoveredMachineWallet = @event.VendingMachineWallet;
            var recoveredBuyerWallet = @event.BuyerWallet;

            Assert.True(!recoveredMachineWallet.Any());
            
            Assert.True(recoveredBuyerWallet.Any());
            Assert.Single(recoveredBuyerWallet, c => c.ParValue == 10 && c.Count == 2);
            Assert.Single(recoveredBuyerWallet, c => c.ParValue == 2 && c.Count == 1);
            Assert.Single(recoveredBuyerWallet, c => c.ParValue == 1 && c.Count == 1);
        }
    }
}
