using System.ComponentModel;

namespace AbcAnalysis.StatisticsModels
{
    /// <summary>
    /// Статистика изменений классов АВС
    /// </summary>
    public readonly struct AbcChangeDirectionStatistics
    {
        [Description("Дата")]
        public DateTime Date { get; }

        public int AB { get; }
        public int AC { get; }
        public int AX { get; }
        public int BA { get; }
        public int BC { get; }
        public int BX { get; }
        public int CA { get; }
        public int CB { get; }
        public int XA { get; }
        public int XB { get; }

        public AbcChangeDirectionStatistics(DateTime date, AbcChangeDirectionStatisticsItem[] items)
        {
            Date = date;
            AB = GetQtySku(items, AbcChangeDirection.AB);
            AC = GetQtySku(items, AbcChangeDirection.AC);
            AX = GetQtySku(items, AbcChangeDirection.AX);
            BA = GetQtySku(items, AbcChangeDirection.BA);
            BC = GetQtySku(items, AbcChangeDirection.BC);
            BX = GetQtySku(items, AbcChangeDirection.BX);
            CA = GetQtySku(items, AbcChangeDirection.CA);
            CB = GetQtySku(items, AbcChangeDirection.CB);
            XA = GetQtySku(items, AbcChangeDirection.XA);
            XB = GetQtySku(items, AbcChangeDirection.XB);
        }

        private int GetQtySku(AbcChangeDirectionStatisticsItem[] items, AbcChangeDirection direction) =>
            Array.Find(items, x => x.Direction == direction).QtySku;
    }
}