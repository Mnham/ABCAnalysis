using AbcAnalysis.Models;

namespace AbcAnalysis
{
    public sealed class AbcCalculator : AbcCalculatorBase
    {
        public AbcCalculator(CalculationParameters parameters, IReadOnlyCollection<OrderData> orders) : base(parameters, orders)
        {
        }

        protected override void CalculateAbcTable(IReadOnlyList<AbcTableItem> abcTable)
        {
            // TODO: Реализовать алгоритм расчета таблицы АВС
            for (int i = 0; i < abcTable.Count; i++)
            {
                abcTable[i].Abc = GetAbc(i);
            }
        }
    }
}