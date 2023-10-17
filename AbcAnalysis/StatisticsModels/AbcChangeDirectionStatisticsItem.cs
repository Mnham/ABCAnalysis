using AbcAnalysis.Models;

namespace AbcAnalysis.StatisticsModels
{
    public readonly struct AbcChangeDirectionStatisticsItem
    {
        /// <summary>
        /// Направление изменения класса АВС
        /// </summary>
        public AbcChangeDirection Direction { get; }

        /// <summary>
        /// Количество Sku
        /// </summary>
        public int QtySku { get; }

        public AbcChangeDirectionStatisticsItem(Abc previousAbc, Abc currentAbc, int qtySku)
        {
            int value = ((int)previousAbc * 10) + (int)currentAbc;
            Direction = (AbcChangeDirection)Enum.ToObject(typeof(AbcChangeDirection), value);
            QtySku = qtySku;
        }
    }
}