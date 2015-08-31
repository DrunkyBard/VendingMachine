using VendingMachineApp.Business;
using VendingMachineApp.Business.Events;

namespace VendingMachineApp.Commands
{
    public sealed class BuyCommandHandler
    {
        public GoodsBuyedEvent Execute(BuyCommand command)
        {
            // Restore aggregate

            var vendingMachine = new VendingMachine(null, null, null);
            var @event = vendingMachine.BuyGoods(command.Deposit, command.Goods);

            // Save event

            return @event;
        }
    }
}
