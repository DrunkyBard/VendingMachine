using System.Linq;
using VendingMachineApp.Business;
using VendingMachineApp.DataAccess.Entities;

namespace VendingMachineApp.DataAccess.EF
{
    internal sealed class VendingMachineEntityFactory : EfContextEntityFactory<VendingMachineDbContext, UserVendingMachineAggregationEntity, NullIdentity>
    {
        public VendingMachineEntityFactory(VendingMachineDbContext dbContext) : base(dbContext)
        {}

        public override UserVendingMachineAggregationEntity Create(NullIdentity identity)
        {
            var vMachine = DbContext.Set<VendingMachineEntity>().Single();
            var user = DbContext.Set<UserEntity>().Single();

            return new UserVendingMachineAggregationEntity(vMachine, user);
        }
    }
}
