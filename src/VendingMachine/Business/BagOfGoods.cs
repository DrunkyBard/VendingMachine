using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using VendingMachineApp.Business.Exceptions;
using VendingMachineApp.Utils;

namespace VendingMachineApp.Business
{
    public sealed class BagOfGoods : IEnumerable<Goods>
    {
        private readonly HashSet<Goods> _bag;

        public BagOfGoods() : this(Enumerable.Empty<Goods>())
        { }

        public BagOfGoods(IEnumerable<Goods> goods)
        {
            Contract.Requires(goods != null);

            var goodsEqualityComparer = EqualityComparerFactory.Create<Goods>(
                (g1, g2) => g1.Identity.Equals(g2.Identity) && g1.Price == g2.Price,
                g => g.Identity.GetHashCode()
                );
            _bag = goods
                .GroupBy(g => new { g.Identity, g.Price })
                .Select(g => g.Aggregate(
                    default(int),
                    (count, nextGoods) => count + nextGoods.Count,
                    c => new Goods(g.Key.Identity, g.Key.Price, c)))
                .ToHashSet(goodsEqualityComparer);
        }

        public Goods Retrieve(GoodsIdentity goods, int count = 1)
        {
            Contract.Requires(count > 0);

            var availableGoods = _bag.SingleOrDefault(g => g.Identity.Equals(goods));

            if (availableGoods == null || availableGoods.Count < count)
            {
                throw new GoodsShortageException(availableGoods, count);
            }

            _bag.Remove(availableGoods);
            availableGoods = new Goods(goods, availableGoods.Price, availableGoods.Count - count);

            if (availableGoods.Count > count)
            {
                _bag.Add(availableGoods);
            }

            return availableGoods;
        }

        [Pure]
        public IReadOnlyCollection<Goods> ShowGoods()
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<Goods>>() != null);

            return _bag.ToImmutableList();
        }

        public IEnumerator<Goods> GetEnumerator()
        {
            return _bag.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
