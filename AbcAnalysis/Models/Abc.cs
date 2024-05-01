namespace AbcAnalysis.Models
{
    /// <summary>
    /// Класс АВС
    /// </summary>
    public enum Abc : byte
    {
        /// <summary>
        /// Класс не определен, новый товар
        /// </summary>
        NA = 0,

        /// <summary>
        /// Класс A, высоколиквидный товар
        /// </summary>
        A = 1,

        /// <summary>
        /// Класс B, среднеликвидный товар
        /// </summary>
        B = 2,

        /// <summary>
        /// Класс C, низколиквидный товар
        /// </summary>
        C = 3,

        /// <summary>
        /// Класс X, неликвидный товар, отсутствуют продажи за выбранный период
        /// </summary>
        X = 4,
    }
}