using System.Linq;
using VendingMachineApp.DataAccess.EF;
using VendingMachineApp.DataAccess.Entities;
using VendingMachineApp.Models;

namespace VendingMachineApp.DataAccess.Queries
{
    public sealed class ShowVendingMachineQuery
    {
        public VendingMachineViewModel Ask()
        {
            using (var dbContext = new VendingMachineDbContext())
            {
                var vMachine = dbContext.Set<VendingMachineEntity>().Single();
                var buyer = dbContext.Set<UserEntity>().Single();
                var vMachineWallet = vMachine.Coins
                    .Select(c => new CoinViewModel
                    {
                        ParValue = c.FaceValue,
                        Count = c.Count
                    }).ToList();
                var buyerWallet = buyer.Coins
                    .Select(c => new CoinViewModel
                    {
                        ParValue = c.FaceValue,
                        Count = c.Count
                    }).ToList();
                var availableGoods = vMachine.Goods
                    .Select(g => new GoodsViewModel
                    {
                        Identity = g.Id,
                        Name = g.Name,
                        Price = g.Price,
                        Count = g.Count
                    }).ToList();

                return new VendingMachineViewModel
                {
                    AvailableGoods = availableGoods,
                    BuyerWallet = buyerWallet,
                    MachineWallet = vMachineWallet
                };
            }
        }
    }
}
