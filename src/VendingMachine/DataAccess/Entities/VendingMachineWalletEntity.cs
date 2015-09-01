namespace VendingMachineApp.DataAccess.Entities
{
    public class VendingMachineWalletEntity
    {
        public int Id { get; set; }

        public decimal FaceValue { get; set; }

        public int Count { get; set; }

        public VendingMachineEntity VendingMachine { get; set; }

        public int VendingMachineId { get; set; }
    }
}
