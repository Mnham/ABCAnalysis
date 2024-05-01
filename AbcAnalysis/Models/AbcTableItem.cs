namespace AbcAnalysis.Models
{
    /// <summary>
    /// Данные по SKU, в таблице АВС
    /// </summary>
    public sealed class AbcTableItem
    {
        /// <summary>
        /// stock keeping unit - складской номер товара
        /// </summary>
        public int Sku { get; }

        /// <summary>
        /// Общее количество заказов
        /// </summary>
        public int TotalOrders { get; }

        /// <summary>
        /// Класс АВС
        /// </summary>
        public Abc Abc { get; set; }

        public AbcTableItem(int sku, int totalOrders)
        {
            Sku = sku;
            TotalOrders = totalOrders;
        }
    }
}