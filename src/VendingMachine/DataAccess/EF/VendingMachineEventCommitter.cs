using System.Diagnostics.Contracts;
using System.Linq;
using VendingMachineApp.Business;
using VendingMachineApp.Business.Events;
using VendingMachineApp.DataAccess.Core;
using VendingMachineApp.DataAccess.Entities;

namespace VendingMachineApp.DataAccess.EF
{
    internal sealed class VendingMachineEventCommitter :
        ITrackedEventCommitter<UserVendingMachineAggregationEntity, GoodsBuyedEvent>,
        ITrackedEventCommitter<UserVendingMachineAggregationEntity, CoinsRefundedEvent>

    {
        private readonly VendingMachineDbContext _dbContext;

        public VendingMachineEventCommitter(VendingMachineDbContext dbContext)
        {
            Contract.Requires(dbContext != null);

            _dbContext = dbContext;
        }

        public void Commit(UserVendingMachineAggregationEntity entity, CoinsRefundedEvent @event)
        {
            UpdateWallets(entity.VendingMachine, entity.User, @event.VendingMachineWallet, @event.BuyerWallet);

            _dbContext.SaveChanges();
        }

        public void Commit(UserVendingMachineAggregationEntity entity, GoodsBuyedEvent @event)
        {
            UpdateWallets(entity.VendingMachine, entity.User, @event.UpdatedMachineWallet, @event.UpdatedBuyerWallet);

            //TODO: Update goods
            _dbContext.SaveChanges();
        }

        private void UpdateWallets(VendingMachineEntity vMachine, UserEntity user, Wallet vendingMachineWallet, Wallet buyerWallet)
        {
            _dbContext.Set<VendingMachineWalletEntity>().RemoveRange(vMachine.Coins);

            var updatedVMachineWallet = vendingMachineWallet
                .Select(x => new VendingMachineWalletEntity
                {
                    FaceValue = x.ParValue,
                    Count = x.Count,
                    VendingMachineId = vMachine.Id
                }).ToList();
            var updatedUserWallet = buyerWallet
                .Select(x => new UserWalletEntity
                {
                    FaceValue = x.ParValue,
                    Count = x.Count,
                    UserId = user.Id
                }).ToList();
            vMachine.Coins = updatedVMachineWallet;
            user.Coins = updatedUserWallet;
        }
    }
}
