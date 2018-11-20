using FileManagement;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Utils.FileManagement
{
    public class ExcelManager : IIoFile<ExcelPackage>
    {
        public static bool IsExcelInstalled()
        {
            return Type.GetTypeFromProgID("Excel.Application") != null;
        }

        
       
        public void AddWorkSheets(StringList worksheetsName,string filename)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                foreach (string name in worksheetsName)
                {
                    AddWorkSheet(excel, name);
                }
                Save(excel);
            }
        }

        public void RangeToBold(string range, string workSheetName, string filename)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = getWorksheet(excel, workSheetName);
                worksheet.Cells[range].Style.Font.Bold = true;
                Save(excel);
            }
        }



        public void ChangeRangeStyle(string range, string workSheetName, string filename,bool bold, System.Drawing.Color color, System.Drawing.Color background, float size)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = getWorksheet(excel, workSheetName);
                ExcelStyle style = worksheet.Cells[range].Style;
                style.Font.Color.SetColor(color);
                style.Fill.BackgroundColor.SetColor(color);
                style.Font.Bold = bold;
                style.Font.Size = size;
                Save(excel);
            }
        }



        public bool CheckRangeBold(string range, string workSheetName, string filename)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = getWorksheet(excel, workSheetName);
                return worksheet.Cells[range].Style.Font.Bold;
            }
        }



        public void RangeToColor(string range, System.Drawing.Color color, string workSheetName, string filename)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = getWorksheet(excel, workSheetName);
                worksheet.Cells[range].Style.Font.Color.SetColor(color);
                Save(excel);
            }
        }


        public System.Drawing.Color CheckRangeColor(string range, string workSheetName, string filename)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = getWorksheet(excel, workSheetName);
                return Color.ToDrawingColor(worksheet.Cells[range].Style.Font.Color);
            }
        }


        public void RangeToBackGroundColor(string range, System.Drawing.Color color, string workSheetName, string filename)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = getWorksheet(excel, workSheetName);
                worksheet.Cells[range].Style.Fill.BackgroundColor.SetColor(color);
                Save(excel);
            }
        }


        public System.Drawing.Color CheckRangeBackgroundColor(string range, string workSheetName, string filename)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = getWorksheet(excel, workSheetName);
                return Color.ToDrawingColor(worksheet.Cells[range].Style.Fill.BackgroundColor);
            }
        }

        public void RangeResize(string range, string workSheetName, string filename, float size)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = getWorksheet(excel, workSheetName);
                worksheet.Cells[range].Style.Font.Size = size;
                Save(excel);
            }
        }


        public float CheckRangeSize(string range, string workSheetName, string filename)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = getWorksheet(excel, workSheetName);
                return worksheet.Cells[range].Style.Font.Size;
            }
        }


        /// <summary>
        /// clear range with save
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="Worksheetname"></param>
        /// <param name="nbColumn"></param>
        /// <param name="nbRow"></param>
        public void Clear(string filename, string Worksheetname, string range)
        {
            using(ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                Clear(excel, Worksheetname, range);
                Save(excel);
            }
        }


        public void Save(ExcelPackage instanceToSave)
        {
            string filename = instanceToSave.File.FullName;
            if (!FileManager.IsFileLocked(filename) || !File.Exists(filename))
            {
                instanceToSave.Save();
            }
            else
            {
                throw new InvalidOperationException(filename + " is already open");
            }
           
        } 

        /// <summary>
        /// clear range without save
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="Worksheetname"></param>
        /// <param name="nbColumn"></param>
        /// <param name="nbRow"></param>
        public static void Clear(ExcelPackage excel, string Worksheetname, string range)
        {
            var worksheet = getWorksheet(excel, Worksheetname);
            worksheet.Cells[range].Clear();
        }

        public static void CreateGraph()
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

        /// <summary>
        /// change cell value with save
        /// </summary>
        /// <param name="worksheetName"></param>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        public void ChangeCellValue(string filename, string worksheetName, string cell, object value)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = excel.Workbook.Worksheets[worksheetName];
                worksheet.Cells[cell].Value = value;
                Save(excel);
            }
        }

        /// <summary>
        /// change cell value without save
        /// </summary>
        /// <param name="worksheetName"></param>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        private static void ChangeCellValue(ExcelPackage excel, string worksheetName, string cell, object value)
        {
            var worksheet = excel.Workbook.Worksheets[worksheetName];
            worksheet.Cells[cell].Value = value;
        }

        /// <summary>
        /// change many cells at same time
        /// and save at the end
        /// </summary>
        /// <param name="worksheetName"></param>
        /// <param name="cellsValues"></param>
        public void ChangeCellsValue(string filename, string worksheetName, IDictionary<string, object> cellsValues)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                foreach (KeyValuePair<string, object> entry in cellsValues)
                {
                    var worksheet = ExcelManager.getWorksheet(excel, worksheetName);
                    ChangeCellValue(excel, worksheet.Name, entry.Key, entry.Value);
                }
                Save(excel);
            }
        }

        public void Open(string fileName)
        {
            if (IsExcelInstalled())
            {
                FileManager.OpenFile(fileName);
            }

        }

    }
}
