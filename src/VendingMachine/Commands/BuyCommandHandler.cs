using System;
using System.Diagnostics.Contracts;
using VendingMachineApp.Business;
using VendingMachineApp.Business.Events;
using VendingMachineApp.DataAccess.Core;
using VendingMachineApp.DataAccess.Entities;

namespace VendingMachineApp.Commands
{
    public sealed class BuyCommandHandler
    {
        private readonly Func<UnitOfWork<UserVendingMachineAggregationEntity>> _uowFactory;

        public BuyCommandHandler(Func<UnitOfWork<UserVendingMachineAggregationEntity>> uowFactory)
        {
            Contract.Requires(uowFactory != null);

            _uowFactory = uowFactory;
        }

        public GoodsBuyedEvent Execute(BuyCommand command)
        {
            // Restore aggregate
            using (var uow = _uowFactory())
            {
                var entity = uow.Get(new NullIdentity());


                var vendingMachine = new VendingMachine(null, null, null);
                var @event = vendingMachine.BuyGoods(command.Deposit, command.Goods);

                // Save event

                return @event;
            }
        }
    }
}
