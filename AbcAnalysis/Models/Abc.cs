namespace AbcAnalysis.Models
{
    /// <summary>
    /// Класс АВС
    /// </summary>
    [Flags]
    public enum Abc : byte
    {
        NA = 0,
        A = 1,
        B = 2,
        C = A | B,
        X = 4,
        ABC = A | B | C
    }
}