using System;

namespace VendingMachineApp.Models
{
    public sealed class GoodsViewModel
    {
        public Guid Identity { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}
