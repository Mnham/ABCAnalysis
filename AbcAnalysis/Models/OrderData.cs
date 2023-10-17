namespace AbcAnalysis.Models
{
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