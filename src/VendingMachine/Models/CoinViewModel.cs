using System;
using System.ComponentModel.DataAnnotations;

namespace VendingMachineApp.Models
{
    public sealed class CoinViewModel
    {
        [Required]
        [DataType(DataType.Currency)]
        public decimal ParValue { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int Count { get; set; }
    }
}
