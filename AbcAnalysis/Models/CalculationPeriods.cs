namespace AbcAnalysis.Models
{
    /// <summary>
    /// Периоды для расчетов
    /// </summary>
    /// <param name="AbcTable">Период для расчета таблицы АВС</param>
    /// <param name="Statistics">Период для расчета статистики по заказам</param>
    public readonly record struct CalculationPeriods(Period AbcTable, Period Statistics);
}