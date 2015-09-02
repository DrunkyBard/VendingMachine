using System;
using System.Diagnostics.Contracts;
using System.Linq;
using VendingMachineApp.Business;
using VendingMachineApp.Business.Events;
using VendingMachineApp.DataAccess.Core;
using VendingMachineApp.DataAccess.Entities;

namespace VendingMachineApp.Commands
{
    public abstract class VendingMachineCommandHandler<TInCommand, TOutEvent> where TOutEvent : IEvent
    {
        private readonly Func<UnitOfWork<UserVendingMachineAggregationEntity>> _uowFactory;

        protected VendingMachineCommandHandler(Func<UnitOfWork<UserVendingMachineAggregationEntity>> uowFactory)
        {
            Contract.Requires(uowFactory != null);

            _uowFactory = uowFactory;
        }

        public TOutEvent Execute(TInCommand command)
        {
            using (var uow = _uowFactory())
            {
                var vMachineEntity = uow.Get(new NullIdentity());
                var machineCoins = vMachineEntity.VendingMachine.Coins
                    .Select(x => new Coin(x.FaceValue, x.Count))
                    .ToArray();
                var buyerCoins = vMachineEntity.User.Coins
                    .Select(x => new Coin(x.FaceValue, x.Count))
                    .ToArray();
                var machineGoods = vMachineEntity.VendingMachine.Goods
                    .Select(x => new Goods(new GoodsIdentity(x.Id), x.Price, x.Count))
                    .ToList();
                var machineWallet = new Wallet(machineCoins);
                var buyerWallet = new Wallet(buyerCoins);

                var aggregate = new VendingMachine(machineWallet, buyerWallet, machineGoods);
                var @event = Execute(aggregate, command);
                Contract.Assume(@event != null);
                uow.Commit(@event);

                return @event;
            }
        }

        protected abstract TOutEvent Execute(VendingMachine vendingMachine, TInCommand command);
    }
}
