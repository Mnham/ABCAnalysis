using System.Globalization;
using AbcAnalysis.Models;
using CsvHelper;

namespace AbcAnalysis.Utils
{
    public static class OrdersProvider
    {
        public static IReadOnlyCollection<OrderData> GetOrders()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orders.csv");
            using StreamReader reader = new(path);
            using CsvReader csv = new(reader, CultureInfo.InvariantCulture);

            return csv.GetRecords<OrderData>().ToArray();
        }
    }
}