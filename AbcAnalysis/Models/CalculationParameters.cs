namespace AbcAnalysis.Models
{
    public sealed class CalculationParameters
    {
        /// <summary>
        /// Количество SKU класса А
        /// </summary>
        public int QtySkuA { get; init; }

        /// <summary>
        /// Количество SKU класса В
        /// </summary>
        public int QtySkuB { get; init; }

        /// <summary>
        /// Интервал запуска АВС-анализа, дней
        /// </summary>
        public int RunInterval { get; init; }

        /// <summary>
        /// Расчетный период таблицы АВС, дней
        /// </summary>
        public int CalculationPeriod { get; init; }
    }
}