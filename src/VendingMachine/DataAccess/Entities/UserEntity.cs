using System.Collections.Generic;

namespace VendingMachineApp.DataAccess.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }

        public virtual ICollection<UserWalletEntity> Coins { get; set; }
    }
}
