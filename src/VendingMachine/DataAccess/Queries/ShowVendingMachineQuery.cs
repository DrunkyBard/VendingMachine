using System;
using VendingMachineApp.Models;

namespace VendingMachineApp.DataAccess.Queries
{
    public sealed class ShowVendingMachineQuery
    {
        public VendingMachineViewModel Ask()
        {
            var goods = new[]
            {
                new GoodsViewModel
                {
                    Identity = Guid.NewGuid(),
                    Name = "A",
                    Count = 2,
                    Price = 1
                },
                new GoodsViewModel
                {
                    Identity = Guid.NewGuid(),
                    Name = "B",
                    Count = 3,
                    Price = 5
                }
            };

            var buyerWallet = new[]
            {
                new CoinViewModel
                {
                    Count = 1,
                    ParValue = 2
                },
                new CoinViewModel
                {
                    Count = 3,
                    ParValue = 5
                },
                new CoinViewModel
                {
                    Count = 7,
                    ParValue = 10
                }, 
            };

            return new VendingMachineViewModel
            {
                AvailableGoods = goods,
                BuyerWallet = buyerWallet,
                MachineWallet = buyerWallet
            };
        }
    }
}
