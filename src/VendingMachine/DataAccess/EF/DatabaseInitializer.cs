using System;
using System.Collections.Generic;
using System.Data.Entity;
using VendingMachineApp.DataAccess.Entities;

namespace VendingMachineApp.DataAccess.EF
{
    internal sealed class DatabaseInitializer : CreateDatabaseIfNotExists<VendingMachineDbContext>
    {
        protected override void Seed(VendingMachineDbContext context)
        {
            var vendingMachine = new VendingMachineEntity
            {
                Coins = new List<VendingMachineWalletEntity>
                {
                    new VendingMachineWalletEntity
                    {
                        FaceValue = 1,
                        Count = 100
                    },
                    new VendingMachineWalletEntity
                                        {
                                            FaceValue = 2,
                                            Count = 100
                                        },
                    new VendingMachineWalletEntity
                                        {
                                            FaceValue = 5,
                                            Count = 100
                                        },
                    new VendingMachineWalletEntity
                    {
                        FaceValue = 10,
                        Count = 100
                    },

                }
            };
            var user = new UserEntity
            {
                Coins = new List<UserWalletEntity>
                {
                    new UserWalletEntity
                    {
                        FaceValue = 1,
                        Count = 10
                    },
                    new UserWalletEntity
                                        {
                                            FaceValue = 2,
                                            Count = 30
                                        },
                    new UserWalletEntity
                                        {
                                            FaceValue = 5,
                                            Count = 20
                                        },
                    new UserWalletEntity
                    {
                        FaceValue = 10,
                        Count = 15
                    },

                }
            };
            var goods = new List<GoodsEntity>
            {
                new GoodsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Чай",
                    Count = 10,
                    Price = 13,
                    VendingMachine = vendingMachine
                },
                new GoodsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Кофе",
                    Count = 20,
                    Price = 18,
                    VendingMachine = vendingMachine
                },
                new GoodsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Кофе с молоком",
                    Count = 20,
                    Price = 21,
                    VendingMachine = vendingMachine
                },
                new GoodsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Сок",
                    Count = 15,
                    Price = 35,
                    VendingMachine = vendingMachine
                }
            };

            context.Set<VendingMachineEntity>().Add(vendingMachine);
            context.Set<UserEntity>().Add(user);
            context.Set<GoodsEntity>().AddRange(goods);

            context.SaveChanges();
        }
    }
}
