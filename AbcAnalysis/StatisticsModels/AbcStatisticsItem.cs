using AbcAnalysis.Models;

namespace AbcAnalysis.StatisticsModels
{
    public readonly struct AbcStatisticsItem
    {
        /// <summary>
        /// Класс АВС
        /// </summary>
        public Abc Abc { get; }

        /// <summary>
        /// Количество Sku
        /// </summary>
        public int QtySku { get; }

        /// <summary>
        /// Общее количество заказов
        /// </summary>
        public int TotalOrders { get; }

        public AbcStatisticsItem(Abc abc, int qtySku, int totalOrders)
        {
            Abc = abc;
            QtySku = qtySku;
            TotalOrders = totalOrders;
        }
    }
}