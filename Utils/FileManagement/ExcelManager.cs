using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.FileManagement
{
    public class ExcelManager
    {
        public bool OpenExcel(string filename)
        {
            bool isExcelInstalled = Type.GetTypeFromProgID("Excel.Application") != null;
            if (isExcelInstalled)
            {
                System.Diagnostics.Process.Start(new FileInfo(filename).ToString());
            }
            return isExcelInstalled;
        }


        public void AddWorkSheets(StringList worksheetsName,string filename)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                foreach (string name in worksheetsName)
                {
                    AddWorkSheet(excel, name);
                }
                excel.Save();
            }
        }

        public void RangeToBold(string range, string workSheetName, string filename)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = getWorksheet(excel, workSheetName);
                worksheet.Cells[range].Style.Font.Bold = true;
                excel.Save();
            }
        }

        public void RangeToColor(string range, Color color, string workSheetName, string filename)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = getWorksheet(excel, workSheetName);
                worksheet.Cells[range].Style.Font.Color.SetColor(color);
                excel.Save();
            }
        }

        public void RangeResize(string range, int size, string filename, string workSheetName)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = getWorksheet(excel, workSheetName);
                worksheet.Cells[range].Style.Font.Size = size;
                excel.Save();
            }
        }

        public static void Clear(ExcelPackage excel, string Worksheetname, int nbColumn, int nbRow)
        {
            var worksheet = getWorksheet(excel, Worksheetname);
            string headerRange = "A1:" + Char.ConvertFromUtf32(nbColumn + 64) + nbRow;
            worksheet.Cells[headerRange].Clear();
        }

        internal void CreateGraph()
        {
            /*ExcelChart chart = chartSheet.Drawings.AddChart("FindingsChart",
            OfficeOpenXml.Drawing.Chart.eChartType.ColumnClustered);
            chart.Title.Text = "Category Chart";
            chart.SetPosition(1, 0, 3, 0);
            chart.SetSize(800, 300);
            var ser1 = (ExcelChartSerie)(chart.Series.Add(workSheet.Cells["B4:B6"],
            workSheet.Cells["A4:A6"]));
            ser1.Header = "Category";*/
        }

        public static ExcelWorksheet AddWorkSheet(ExcelPackage excel, string worksheetName)
        {
            excel.Workbook.Worksheets.Add(worksheetName);
            return excel.Workbook.Worksheets[worksheetName];
        }

        public static ExcelWorksheet getWorksheet(ExcelPackage excel, string name)
        {
            var worksheet = excel.Workbook.Worksheets[name];
            if (worksheet == null)
            {
                worksheet = AddWorkSheet(excel, name);
            }
            return worksheet;
        }
    }
}
