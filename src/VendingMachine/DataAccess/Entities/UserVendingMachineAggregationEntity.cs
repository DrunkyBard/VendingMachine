using System.Diagnostics.Contracts;

namespace VendingMachineApp.DataAccess.Entities
{
    public sealed class UserVendingMachineAggregationEntity
    {
        public readonly VendingMachineEntity VendingMachine;
        public readonly UserEntity User;

        public UserVendingMachineAggregationEntity(VendingMachineEntity vendingMachine, UserEntity user)
        {
            Contract.Requires(vendingMachine != null);
            Contract.Requires(user != null);

            VendingMachine = vendingMachine;
            User = user;
        }
    }
}
