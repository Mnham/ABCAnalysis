namespace AbcAnalysis.Models
{
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

        public AbcTableItem(int sku, int totalOrdersCount)
        {
            Sku = sku;
            TotalOrders = totalOrdersCount;
        }
    }
}