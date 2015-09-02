using System.Collections.Generic;

namespace VendingMachineApp.DataAccess.Entities
{
    public class VendingMachineEntity
    {
        public int Id { get; set; }

        public virtual ICollection<VendingMachineWalletEntity> Coins { get; set; }

        public virtual ICollection<GoodsEntity> Goods { get; set; }
    }
}
