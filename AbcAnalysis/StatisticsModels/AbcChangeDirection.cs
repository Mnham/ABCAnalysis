namespace AbcAnalysis.StatisticsModels
{
    /// <summary>
    /// Направление изменения класса АВС
    /// </summary>
    public enum AbcChangeDirection : byte
    {
        NAA = 1,
        NAB = 2,
        NAC = 3,
        AA = 11,
        AB = 12,
        AC = 13,
        AX = 14,
        BA = 21,
        BB = 22,
        BC = 23,
        BX = 24,
        CA = 31,
        CB = 32,
        CC = 33,
        CX = 34,
        XA = 41,
        XB = 42,
        XC = 43,
        XX = 44
    }
}