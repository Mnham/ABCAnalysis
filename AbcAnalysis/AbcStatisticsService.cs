using System.Drawing;
using AbcAnalysis.Models;
using AbcAnalysis.StatisticsModels;
using AbcAnalysis.Utils;
using OfficeOpenXml;
using OfficeOpenXml.ConditionalFormatting.Contracts;

namespace AbcAnalysis
{
    public sealed class AbcStatisticsService
    {
        private readonly AbcCalculatorBase _abcCalculator;

        /// <summary>
        /// Изменения классов АВС
        /// </summary>
        public List<AbcChangeDirectionStatistics> AbcDirectionChanges { get; } = [];

        /// <summary>
        /// Количество SKU в таблице АВС по классам
        /// </summary>
        public List<AbcStatistics> AbcTableQtySku { get; } = [];

        /// <summary>
        /// Количество заказов в таблице АВС по классам
        /// </summary>
        public List<AbcStatistics> AbcTableTotalOrders { get; } = [];

        /// <summary>
        /// Процент SKU в таблице АВС по классам
        /// </summary>
        public List<AbcStatistics> AbcTablePercentQtySku { get; } = [];

        /// <summary>
        /// Процент заказов в таблице АВС по классам
        /// </summary>
        public List<AbcStatistics> AbcTablePercentTotalOrders { get; } = [];

        /// <summary>
        /// Количество заказанных SKU
        /// </summary>
        public List<AbcStatistics> QtySkuInOrders { get; } = [];

        /// <summary>
        /// Количество заказов
        /// </summary>
        public List<AbcStatistics> TotalOrders { get; } = [];

        /// <summary>
        /// Процент заказанных SKU
        /// </summary>
        public List<AbcStatistics> PercentQtySkuInOrders { get; } = [];

        /// <summary>
        /// Процент заказов
        /// </summary>
        public List<AbcStatistics> PercentTotalOrders { get; } = [];

        public AbcStatisticsService(AbcCalculatorBase abcCalculator)
        {
            _abcCalculator = abcCalculator;
            abcCalculator.AbcTableCalculationCompleted += RecordStatistics;
        }

        public FileInfo CreateExcelReport(string directory)
        {
            string path = Path.Combine(directory, $"Статистика_{DateTime.Now:fffffff}.xlsx");
            FileInfo file = new(path);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage package = new(file);
            ExcelWorksheets worksheets = package.Workbook.Worksheets;

            WorksheetHelper.Get(worksheets, "Заказы")
                .AddLineChart(QtySkuInOrders, "Количество заказанных SKU")
                .AddLineChart(TotalOrders, "Количество заказов", isEndChartLine: true)
                .AddLineChart(PercentQtySkuInOrders, "Процент заказанных SKU")
                .AddLineChart(PercentTotalOrders, "Процент заказов");

            WorksheetHelper.Get(worksheets, "Таблица АВС")
                .AddLineChart(AbcTableQtySku, "Количество SKU в таблице АВС по классам")
                .AddLineChart(AbcTableTotalOrders, "Количество заказов в таблице АВС по классам", isEndChartLine: true)
                .AddLineChart(AbcTablePercentQtySku, "Процент SKU в таблице АВС по классам")
                .AddLineChart(AbcTablePercentTotalOrders, "Процент заказов в таблице АВС по классам");

            WorksheetHelper.Get(worksheets, "Изменения АВС")
                .AddLineChart(AbcDirectionChanges, width: 1792, height: 760);

            ExcelWorksheet wrksheet = worksheets.Add("Хронология АВС");
            int rows = _abcCalculator.AbcMatrix.Count + 1;
            int columns = _abcCalculator.Iterations.Count + 1;
            wrksheet.SetValue(1, 1, "SKU");
            for (int column = 2; column <= columns; column++)
            {
                wrksheet.SetValue(1, column, column - 1);
            }

            int row = 2;
            foreach (KeyValuePair<int, Abc[]> item in _abcCalculator.AbcMatrix)
            {
                int column = 2;
                wrksheet.SetValue(row, 1, item.Key);
                foreach (Abc abc in item.Value)
                {
                    wrksheet.SetValue(row, column++, abc);
                }

                row++;
            }

            ExcelAddress address = new(2, 2, rows, columns);
            SetConditionalFormatting(wrksheet, address, "NA", 0, 176, 240);
            SetConditionalFormatting(wrksheet, address, "A", 146, 208, 80);
            SetConditionalFormatting(wrksheet, address, "B", 255, 192, 0);
            SetConditionalFormatting(wrksheet, address, "C", 237, 125, 49);
            SetConditionalFormatting(wrksheet, address, "X", 192, 0, 0);

            package.Save();

            return package.File;
        }

        private void RecordStatistics(IEnumerable<AbcTableItem> abcTable)
        {
            IReadOnlyDictionary<int, Abc[]> abcMatrix = _abcCalculator.AbcMatrix;
            DateTime endAbcTablePeriod = _abcCalculator.CurrentCalculationPeriods.AbcTable.End;
            int previousIteration = _abcCalculator.PreviousIteration;
            int currentIteration = _abcCalculator.CurrentIteration;

            AbcChangeDirectionStatisticsItem[] abcChangeDirectionItems = abcMatrix.Values
                .Where(m => m[currentIteration] != Abc.NA && m[previousIteration] != m[currentIteration])
                .GroupBy(
                    k => new { PreviousAbc = k[previousIteration], CurrentAbc = k[currentIteration] },
                    (k, g) => new AbcChangeDirectionStatisticsItem(k.PreviousAbc, k.CurrentAbc, g.Count()))
                .ToArray();

            AbcDirectionChanges.Add(new AbcChangeDirectionStatistics(endAbcTablePeriod, abcChangeDirectionItems));

            AbcStatisticsItem[] abcTableStatistics = abcMatrix.Values
                .Where(x => x[currentIteration] == Abc.X)
                .GroupBy(k => k[currentIteration], (k, g) => new AbcStatisticsItem(k, g.Count(), 0))
                .Concat(abcTable.GroupBy(k => k.Abc, (k, g) => new AbcStatisticsItem(k, g.Count(), g.Sum(x => x.TotalOrders))))
                .ToArray();

            AbcTableQtySku.Add(new AbcStatistics(endAbcTablePeriod, abcTableStatistics, x => x.QtySku, StatisticsType.Value));
            AbcTableTotalOrders.Add(new AbcStatistics(endAbcTablePeriod, abcTableStatistics, x => x.TotalOrders, StatisticsType.Value));
            AbcTablePercentQtySku.Add(new AbcStatistics(endAbcTablePeriod, abcTableStatistics, x => x.QtySku, StatisticsType.Percent));
            AbcTablePercentTotalOrders.Add(new AbcStatistics(endAbcTablePeriod, abcTableStatistics, x => x.TotalOrders, StatisticsType.Percent));

            if (currentIteration == _abcCalculator.Iterations.Count - 1)
            {
                return;
            }

            DateTime startStatistics = _abcCalculator.CurrentCalculationPeriods.Statistics.Start;
            DateTime endStatistics = _abcCalculator.CurrentCalculationPeriods.Statistics.End;

            AbcStatisticsItem[] ordersStatistics =
                (from x in from o in _abcCalculator.Orders
                           join m in abcMatrix on o.Sku equals m.Key
                           where o.Date >= startStatistics && o.Date <= endStatistics
                           group o by new { o.Sku, Abc = m.Value[currentIteration] } into g
                           select new { g.Key, TotalOrders = g.Sum(x => x.Orders) }
                 group x by x.Key.Abc into g
                 select new AbcStatisticsItem(g.Key, g.Count(), g.Sum(x => x.TotalOrders)))
                    .ToArray();

            QtySkuInOrders.Add(new AbcStatistics(endStatistics, ordersStatistics, x => x.QtySku, StatisticsType.Value));
            TotalOrders.Add(new AbcStatistics(endStatistics, ordersStatistics, x => x.TotalOrders, StatisticsType.Value));
            PercentQtySkuInOrders.Add(new AbcStatistics(endStatistics, ordersStatistics, x => x.QtySku, StatisticsType.Percent));
            PercentTotalOrders.Add(new AbcStatistics(endStatistics, ordersStatistics, x => x.TotalOrders, StatisticsType.Percent));
        }

        private void SetConditionalFormatting(ExcelWorksheet wrksheet, ExcelAddress address, string text, int red, int green, int blue)
        {
            IExcelConditionalFormattingContainsText containsText = wrksheet.ConditionalFormatting.AddContainsText(address);
            containsText.Text = text;
            containsText.Style.Fill.BackgroundColor.Color = Color.FromArgb(red, green, blue);
        }
    }
}