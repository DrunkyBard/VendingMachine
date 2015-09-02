using System;
using VendingMachineApp.Business;
using VendingMachineApp.Business.Events;
using VendingMachineApp.DataAccess.Core;
using VendingMachineApp.DataAccess.Entities;

namespace VendingMachineApp.Commands
{
    public sealed class RefundCommandHandler : VendingMachineCommandHandler<BuyCommand, CoinsRefundedEvent>
    {
        public RefundCommandHandler(Func<UnitOfWork<UserVendingMachineAggregationEntity>> uowFactory) : base(uowFactory)
        {}

        protected override CoinsRefundedEvent Execute(VendingMachine vendingMachine, BuyCommand command)
        {
            return vendingMachine.Refund(command.Deposit);
        }
    }
}