using System;
using VendingMachineApp.Business;
using VendingMachineApp.Business.Events;
using VendingMachineApp.DataAccess.Core;
using VendingMachineApp.DataAccess.Entities;

namespace VendingMachineApp.Commands
{
    public sealed class BuyCommandHandler: VendingMachineCommandHandler<BuyCommand, GoodsBuyedEvent>
    {
        public BuyCommandHandler(Func<UnitOfWork<UserVendingMachineAggregationEntity>> uowFactory) : base(uowFactory)
        {}

        protected override GoodsBuyedEvent Execute(VendingMachine vendingMachine, BuyCommand command)
        {
            return vendingMachine.BuyGoods(command.Deposit, command.Goods);
        }
    }
}
