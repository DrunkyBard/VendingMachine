using System.Data.Entity;
using System.Linq;
using VendingMachineApp.Business;
using VendingMachineApp.Business.Events;
using VendingMachineApp.DataAccess.Entities;

namespace VendingMachineApp.DataAccess.EF
{
    internal sealed class GoodsBuyedEventCommitter : EntityFrameworkTrackedEventCommitter<UserVendingMachineAggregationEntity, GoodsBuyedEvent>
    {
        public GoodsBuyedEventCommitter(DbContext dbContext) : base(dbContext)
        { }

        public override void Commit(UserVendingMachineAggregationEntity entity, GoodsBuyedEvent @event)
        {
            UpdateWallets(entity.VendingMachine, entity.User, @event.UpdatedMachineWallet, @event.UpdatedBuyerWallet);

            //TODO: Update goods
            DbContext.SaveChanges();
        }

        private void UpdateWallets(VendingMachineEntity vMachine, UserEntity user, Wallet vendingMachineWallet, Wallet buyerWallet)
        {
            DbContext.Set<VendingMachineWalletEntity>().RemoveRange(vMachine.Coins);

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

    internal sealed class CoinsRefundedEventCommitter : EntityFrameworkTrackedEventCommitter<UserVendingMachineAggregationEntity, CoinsRefundedEvent>
    {
        public CoinsRefundedEventCommitter(DbContext dbContext) : base(dbContext)
        { }

        public override void Commit(UserVendingMachineAggregationEntity entity, CoinsRefundedEvent @event)
        {
            UpdateWallets(entity.VendingMachine, entity.User, @event.VendingMachineWallet, @event.BuyerWallet);

            DbContext.SaveChanges();
        }

        private void UpdateWallets(VendingMachineEntity vMachine, UserEntity user, Wallet vendingMachineWallet, Wallet buyerWallet)
        {
            DbContext.Set<VendingMachineWalletEntity>().RemoveRange(vMachine.Coins);

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
