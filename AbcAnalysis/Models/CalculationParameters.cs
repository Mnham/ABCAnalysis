namespace AbcAnalysis.Models
{
    /// <summary>
    /// Параметры расчета АВС-анализа
    /// </summary>
    public sealed class CalculationParameters
    {
        /// <summary>
        /// Целевое количество SKU класса А, в таблице ABC
        /// </summary>
        public int QtySkuA { get; init; }

        /// <summary>
        /// Целевое количество SKU класса В, в таблице ABC
        /// </summary>
        public int QtySkuB { get; init; }

        /// <summary>
        /// Интервал запуска АВС-анализа, дней
        /// <para>например, каждые 7 дней</para>
        /// </summary>
        public int RunInterval { get; init; }

        /// <summary>
        /// Расчетный период таблицы АВС, дней
        /// <para>период, по которому суммируются заказы по каждому SKU</para>
        /// </summary>
        public int CalculationPeriod { get; init; }
    }
}