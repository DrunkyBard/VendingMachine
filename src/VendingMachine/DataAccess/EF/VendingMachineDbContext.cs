using System.Data.Entity;
using VendingMachineApp.DataAccess.Entities;

namespace VendingMachineApp.DataAccess.EF
{
    public sealed class VendingMachineDbContext : DbContext
    {
        public VendingMachineDbContext()
        {
            Database.SetInitializer(new DatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VendingMachineEntity>()
                .HasKey(x => x.Id)
                .ToTable("VendingMachine");

            modelBuilder.Entity<VendingMachineWalletEntity>()
                .HasKey(x => x.Id)
                .ToTable("VendingMachineWallet");

            modelBuilder.Entity<UserEntity>()
                .HasKey(x => x.Id)
                .ToTable("User");

            modelBuilder.Entity<UserWalletEntity>()
                .HasKey(x => x.Id)
                .ToTable("UserWallet");

            modelBuilder.Entity<GoodsEntity>()
                .HasKey(x => x.Id)
                .ToTable("Goods");
        }
    }
}
