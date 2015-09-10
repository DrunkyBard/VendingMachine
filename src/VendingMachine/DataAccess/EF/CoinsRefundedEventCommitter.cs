using System.Data.Entity;
using System.Linq;
using VendingMachineApp.Business;
using VendingMachineApp.Business.Events;
using VendingMachineApp.DataAccess.Entities;

namespace VendingMachineApp.DataAccess.EF
{
    internal sealed class CoinsRefundedEventCommitter : EntityFrameworkTrackedEventCommitter<UserVendingMachineAggregationEntity, CoinsRefundedEvent>
    {
        public CoinsRefundedEventCommitter(VendingMachineDbContext dbContext) : base(dbContext)
        { }

        public override void Commit(UserVendingMachineAggregationEntity entity, CoinsRefundedEvent @event)
        {
            UpdateWallets(entity.VendingMachine, entity.User, @event.VendingMachineWallet, @event.BuyerWallet);

            DbContext.SaveChanges();
        }

        private void UpdateWallets(VendingMachineEntity vMachine, UserEntity user, Wallet vendingMachineWallet, Wallet buyerWallet)
        {
            DbContext.Set<VendingMachineWalletEntity>().RemoveRange(vMachine.Coins);
            DbContext.Set<UserWalletEntity>().RemoveRange(user.Coins);

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
