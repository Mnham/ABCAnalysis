using System.Diagnostics;
using AbcAnalysis.Models;
using AbcAnalysis.Utils;

namespace AbcAnalysis
{
    static class Program
    {
        static void Main()
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

            Console.WriteLine($"""
                Средний процент заказов: {statisticsService.PercentTotalOrders.Average(x => x.A + x.B):P2}
                Сумма изменений АВС: {statisticsService.AbcDirectionChanges.Sum(x => x.AB + x.AC + x.AX + x.BA + x.BC + x.BX)}
                """);

            FileInfo reportFile = statisticsService.CreateExcelReport(AppDomain.CurrentDomain.BaseDirectory);
            ShowExcelReport(reportFile);
        }

        static void ShowExcelReport(FileInfo reportFile)
        {
            Process excel = new()
            {
                StartInfo = new ProcessStartInfo(reportFile.FullName)
                {
                    UseShellExecute = true
                }
            };

            excel.Start();
        }
    }
}