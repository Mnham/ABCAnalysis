using AbcAnalysis.Models;

namespace AbcAnalysis.StatisticsModels
{
    /// <summary>
    /// Представляет количество SKU, поменявших свой класс
    /// <para>например из A в B, 30 SKU</para>
    /// </summary>
    public readonly struct AbcChangeDirectionStatisticsItem
    {
        /// <summary>
        /// Направление изменения класса АВС
        /// </summary>
        public AbcChangeDirection Direction { get; }

        /// <summary>
        /// Количество SKU
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