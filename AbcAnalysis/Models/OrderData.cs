namespace AbcAnalysis.Models
{
    /// <summary>
    /// Исходные данные по заказам SKU
    /// </summary>
    public sealed class OrderData
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// stock keeping unit - складской номер товара
        /// </summary>
        public int Sku { get; set; }

        /// <summary>
        /// Количество заказов
        /// </summary>
        public int Orders { get; set; }
    }
}