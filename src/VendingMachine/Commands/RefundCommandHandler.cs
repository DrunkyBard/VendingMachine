using System;
using VendingMachineApp.Business;
using VendingMachineApp.Business.Events;
using VendingMachineApp.DataAccess.Core;
using VendingMachineApp.DataAccess.Entities;

namespace VendingMachineApp.Commands
{
    public sealed class RefundCommandHandler : VendingMachineCommandHandler<RefundCommand, CoinsRefundedEvent>
    {
        public RefundCommandHandler(Func<UnitOfWork<UserVendingMachineAggregationEntity>> uowFactory) : base(uowFactory)
        {}

        protected override CoinsRefundedEvent Execute(VendingMachine vendingMachine, RefundCommand command)
        {
            return vendingMachine.Refund(command.Deposit);
        }
    }
}