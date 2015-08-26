using System.Diagnostics.Contracts;

namespace VendingMachine.Business
{
    public sealed class Goods
    {
        public string Name;
        public decimal Price;

        public Goods(string name, decimal price)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            Contract.Requires(price > decimal.Zero);

            Name = name;
            Price = price;
        }
    }
}
