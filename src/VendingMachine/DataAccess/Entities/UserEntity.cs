using System.Collections.Generic;

namespace VendingMachineApp.DataAccess.Entities
{
    public sealed class UserEntity
    {
        public int Id { get; set; }

        public virtual ICollection<UserWalletEntity> Coins { get; set; }
    }
}
