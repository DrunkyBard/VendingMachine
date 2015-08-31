using VendingMachineApp.Business;
using VendingMachineApp.Business.Events;

namespace VendingMachineApp.Commands
{
    public sealed class RefundCommandHandler
    {
        public CoinsRefundedEvent Execute(RefundCommand command)
        {
            //Restore aggregate

            var vendingMachine = new VendingMachine(null, null, null);
            var @event = vendingMachine.Refund(command.Deposit);

            //Save event

            return @event;
        }
    }
}
