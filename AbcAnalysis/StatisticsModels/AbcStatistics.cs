using System.ComponentModel;
using AbcAnalysis.Models;

namespace AbcAnalysis.StatisticsModels
{
    /// <summary>
    /// Аггрегирует статистику по классам ABC, в количественном или в процентном соотношении, по количеству SKU или по количеству заказов
    /// </summary>
    public readonly struct AbcStatistics
    {
        [Description("Дата")]
        public DateTime Date { get; }

        public double A { get; }
        public double B { get; }
        public double C { get; }
        public double X { get; }
        public double NA { get; }

        public AbcStatistics(DateTime date, AbcStatisticsItem[] items, Func<AbcStatisticsItem, double> selector, StatisticsType statisticsType)
        {
            Date = date;

            switch (statisticsType)
            {
                case StatisticsType.Value:
                    A = GetValue(items, Abc.A, selector);
                    B = GetValue(items, Abc.B, selector);
                    C = GetValue(items, Abc.C, selector);
                    X = GetValue(items, Abc.X, selector);
                    NA = GetValue(items, Abc.NA, selector);

                    break;

                case StatisticsType.Percent:
                    double sum = items.Sum(selector);
                    if (sum != 0)
                    {
                        A = GetValue(items, Abc.A, selector) / sum;
                        B = GetValue(items, Abc.B, selector) / sum;
                        C = GetValue(items, Abc.C, selector) / sum;
                        X = GetValue(items, Abc.X, selector) / sum;
                        NA = GetValue(items, Abc.NA, selector) / sum;
                    }

                    break;
            }
        }

        private double GetValue(AbcStatisticsItem[] items, Abc abc, Func<AbcStatisticsItem, double> selector) =>
            selector(Array.Find(items, x => x.Abc == abc));
    }
}