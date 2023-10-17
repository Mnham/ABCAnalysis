using AbcAnalysis.Models;
using AbcAnalysis.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbcAnalysis.Test
{
    [TestClass]
    public class AbcCalculatorTest
    {
        [TestMethod]
        public void TestCalculationResult()
        {
            IReadOnlyCollection<OrderData> orders = OrdersProvider.GetOrders();
            CalculationParameters parameters = new()
            {
                QtySkuA = 60,
                QtySkuB = 140,
                RunInterval = 7,
                CalculationPeriod = 91,
            };

            AbcCalculator calculator = new(parameters, orders);
            AbcStatisticsService statisticsService = new(calculator);
            calculator.Calculate();

            double averagePercentTotalOrders = statisticsService.PercentTotalOrders.Average(x => x.A + x.B);
            int sumAbcDirectionChanges = statisticsService.AbcDirectionChanges.Sum(x => x.AB + x.AC + x.AX + x.BA + x.BC + x.BX);

            Assert.IsTrue(statisticsService.AbcTableQtySku.All(x => x.A == parameters.QtySkuA), "Количество SKU класса А не совпадает с заданным");
            Assert.IsTrue(statisticsService.AbcTableQtySku.All(x => x.B == parameters.QtySkuB), "Количество SKU класса В не совпадает с заданным");
            Assert.IsTrue(averagePercentTotalOrders >= 0.90, "Средний процент заказов: {0}", averagePercentTotalOrders);
            Assert.IsTrue(sumAbcDirectionChanges <= 210, "Сумма изменений АВС: {0}", sumAbcDirectionChanges);
        }
    }
}