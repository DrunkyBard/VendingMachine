using System.Collections.Generic;

namespace VendingMachineApp.Models
{
    public sealed class VendingMachineViewModel
    {
        public ICollection<CoinViewModel> BuyerWallet { get; set; }

        public ICollection<CoinViewModel> MachineWallet { get; set; }

        public ICollection<GoodsViewModel> AvailableGoods { get; set; }
    }
}
