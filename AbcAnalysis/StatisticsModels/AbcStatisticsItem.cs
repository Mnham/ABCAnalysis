using AbcAnalysis.Models;

namespace AbcAnalysis.StatisticsModels
{
    /// <summary>
    /// Статистика по классу ABC
    /// </summary>
    public readonly struct AbcStatisticsItem
    {
        /// <summary>
        /// Класс АВС
        /// </summary>
        public Abc Abc { get; }

        /// <summary>
        /// Количество SKU
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