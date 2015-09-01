namespace VendingMachineApp.DataAccess.Core
{
    public interface IEntityFactory<out TEntity, in TIdentity>
    {
        TEntity Create(TIdentity identity);
    }
}
