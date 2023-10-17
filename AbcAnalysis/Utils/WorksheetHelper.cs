using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace AbcAnalysis.Utils
{
    public sealed class WorksheetHelper
    {
        private readonly ExcelWorksheet _worksheet;
        private int _column = 1;
        private int _rowPosition;
        private int _columnPosition;

        private WorksheetHelper(ExcelWorksheet worksheet)
        {
            _worksheet = worksheet;
        }

        public static WorksheetHelper Get(ExcelWorksheets worksheets, string worksheetName) => new(worksheets.Add(worksheetName));

        public WorksheetHelper AddLineChart<T>(IEnumerable<T> collection, string name = "", bool isEndChartLine = false, int width = 896, int height = 380)
        {
            ExcelRangeBase dataRange = _worksheet.Cells[ExcelCellBase.GetAddress(1, _column)].LoadFromCollection(collection);
            _worksheet.Column(dataRange.Start.Column).Style.Numberformat.Format = "DD.MM.YYYY";

            ExcelChart chart = _worksheet.Drawings.AddChart(name, eChartType.Line);
            chart.Title.Text = name;
            chart.SetPosition(_rowPosition, 0, _columnPosition, 0);
            chart.SetSize(width, height);

            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 1; i < properties.Length; i++)
            {
                ExcelRangeBase serie = dataRange.Offset(0, i, dataRange.End.Row, 1);
                ExcelRangeBase xSerie = dataRange.Offset(0, 0, dataRange.End.Row, 1);
                chart.Series.Add(serie, xSerie).Header = properties[i].Name;
            }

            _column += properties.Length;
            if (isEndChartLine)
            {
                _rowPosition += height / 20;
                _columnPosition = 0;
            }
            else
            {
                _columnPosition += width / 64;
            }

            return this;
        }
    }
}