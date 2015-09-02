using System;

namespace VendingMachineApp.DataAccess.Entities
{
    public class GoodsEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public VendingMachineEntity VendingMachine { get; set; }

        public int VendingMachineId { get; set; }
    }
}
