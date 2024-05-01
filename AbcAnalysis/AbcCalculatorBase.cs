using AbcAnalysis.Models;
using AbcAnalysis.Utils;

namespace AbcAnalysis
{
    public abstract class AbcCalculatorBase
    {
        /// <summary>
        /// Текущая итерация
        /// </summary>
        public int CurrentIteration { get; private set; }

        /// <summary>
        /// Предыдущая итерация
        /// </summary>
        public int PreviousIteration => CurrentIteration - 1;

        /// <summary>
        /// Матрица АВС, здесь сохраняются результаты всех пересчетов.
        /// Матрица выводится на страницу Excel "Хронология АВС"
        /// </summary>
        public IReadOnlyDictionary<int, Abc[]> AbcMatrix { get; }

        /// <summary>
        /// Итераций пересчета таблиц АВС
        /// </summary>
        public List<CalculationPeriods> Iterations { get; } = new();

        /// <summary>
        /// Данные по заказам
        /// </summary>
        public IReadOnlyCollection<OrderData> Orders { get; }

        /// <summary>
        /// Текущие периоды для расчета таблицы АВС и расчета статистики по заказам
        /// </summary>
        public CalculationPeriods CurrentCalculationPeriods { get; private set; }

        /// <summary>
        /// Целевое количество SKU класса А, в таблице ABC
        /// </summary>
        protected int QtySkuA { get; }

        /// <summary>
        /// Целевое количество SKU класса В, в таблице ABC
        /// </summary>
        protected int QtySkuB { get; }

        /// <summary>
        /// Целевое количество SKU классов А и В, в таблице ABC
        /// </summary>
        protected int QtySkuAB { get; }

        public event Action<IEnumerable<AbcTableItem>>? AbcTableCalculationCompleted;

        protected AbcCalculatorBase(CalculationParameters parameters, IReadOnlyCollection<OrderData> orders)
        {
            QtySkuA = parameters.QtySkuA;
            QtySkuB = parameters.QtySkuB;
            QtySkuAB = QtySkuA + QtySkuB;
            Orders = orders;

            DateTime initialDate = orders.Min(x => x.Date);
            DateTime finalDate = orders
                .Where(x => x.Date.DayOfWeek is DayOfWeek.Friday)
                .Max(x => x.Date);

            int runInterval = parameters.RunInterval;
            int calculationPeriod = parameters.CalculationPeriod - 1;
            int iterations = ((int)(finalDate - initialDate).TotalDays - calculationPeriod) / runInterval;
            DateTime startDate = finalDate.AddDays(-((iterations * runInterval) + calculationPeriod));

            iterations++;
            for (int i = 0; i < iterations; i++)
            {
                DateTime endDate = startDate.AddDays(calculationPeriod);

                Period abcTable = new(startDate, endDate);
                Period statistics = new(endDate.AddDays(1), endDate.AddDays(runInterval));
                Iterations.Add(new CalculationPeriods(abcTable, statistics));

                startDate = startDate.AddDays(runInterval);
            }

            AbcMatrix = Orders
                .Distinct(new OrderDataComparer())
                .ToDictionary(x => x.Sku, _ => new Abc[iterations]);
        }

        public void Calculate()
        {
            IReadOnlyList<AbcTableItem> abcTable = CalculateFirstAbcTable(Iterations[0].AbcTable);
            IReadOnlyList<AbcTableItem> previousAbcTable = abcTable;

            foreach (CalculationPeriods periods in Iterations.Skip(1))
            {
                CurrentIteration++;
                CurrentCalculationPeriods = periods;
                abcTable = GetAbcTable(periods.AbcTable);

                CalculateAbcTable(abcTable);

                foreach (AbcTableItem item in abcTable)
                {
                    SetCurrentAbcInMatrix(item.Sku, item.Abc);
                }

                SetAbcX(previousAbcTable);
                previousAbcTable = abcTable;

                AbcTableCalculationCompleted?.Invoke(abcTable);
            }
        }

        /// <summary>
        /// Рассчитывает таблицу АВС
        /// </summary>
        protected abstract void CalculateAbcTable(IReadOnlyList<AbcTableItem> abcTable);

        /// <summary>
        /// Возвращает класс АВС, по порядковому номеру SKU в таблице АВС
        /// </summary>
        protected Abc GetAbc(int skuSequenceNumber)
        {
            if (skuSequenceNumber < QtySkuA)
            {
                return Abc.A;
            }
            else if (skuSequenceNumber < QtySkuAB)
            {
                return Abc.B;
            }

            return Abc.C;
        }

        /// <summary>
        /// Устанавливает в матрице класс АВС для SKU, в текущей итерации
        /// </summary>
        private void SetCurrentAbcInMatrix(int sku, Abc abc) => AbcMatrix[sku][CurrentIteration] = abc;

        /// <summary>
        /// Возвращает таблицу АВС, подготовленную для расчета, классы АВС не определены
        /// </summary>
        private IReadOnlyList<AbcTableItem> GetAbcTable(Period period) => Orders
            .Where(x => x.Date >= period.Start && x.Date <= period.End)
            .GroupBy(x => x.Sku, (k, g) => new AbcTableItem(k, g.Sum(x => x.Orders)))
            .OrderByDescending(x => x.TotalOrders)
            .ThenBy(x => x.Sku)
            .ToList();

        /// <summary>
        /// Возвращает первую таблицу АВС, рассчитанную по стандартному алгоритму
        /// </summary>
        private IReadOnlyList<AbcTableItem> CalculateFirstAbcTable(Period period)
        {
            IReadOnlyList<AbcTableItem> abcTable = GetAbcTable(period);

            for (int i = 0; i < abcTable.Count; i++)
            {
                Abc abc = GetAbc(i);
                abcTable[i].Abc = abc;
                SetCurrentAbcInMatrix(abcTable[i].Sku, abc);
            }

            return abcTable;
        }

        /// <summary>
        /// Устанавливает класс X, для SKU, по которым не было заказов за расчетный период
        /// </summary>
        private void SetAbcX(IEnumerable<AbcTableItem> previousAbcTable)
        {
            foreach (AbcTableItem item in previousAbcTable)
            {
                if (AbcMatrix[item.Sku][CurrentIteration] == Abc.NA)
                {
                    SetCurrentAbcInMatrix(item.Sku, Abc.X);
                }
            }

            foreach (Abc[] item in AbcMatrix.Values)
            {
                if (item[PreviousIteration] == Abc.X && item[CurrentIteration] == Abc.NA)
                {
                    item[CurrentIteration] = Abc.X;
                }
            }
        }
    }
}