using AbcAnalysis.Models;

namespace AbcAnalysis.Utils
{
    public sealed class OrderDataComparer : IEqualityComparer<OrderData>
    {
        public bool Equals(OrderData x, OrderData y) => x.Sku == y.Sku;
        public int GetHashCode(OrderData obj) => obj.Sku.GetHashCode();
    }
}