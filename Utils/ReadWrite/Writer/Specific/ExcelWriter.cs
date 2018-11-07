using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using Utils.FileManagement;
using Utils.ReadWrite.Reader;
using Utils.ReadWrite.Serialization;
using Utils.ReadWrite.Serialization.SpecificSerializer;

namespace Utils.ReadWrite.Writer.Specific
{
    public class ExcelWriter<T> : AbstractWriter<T> where T: Serializable
    {
        //https://www.codebyamir.com/blog/create-excel-files-in-c-sharp
        
        private readonly string _WorkSheetName;

        private readonly StringList _PropertiesToSerialize;


        public ExcelWriter(string workSheetName,StringList propertiesToSerialize)
        {
            _WorkSheetName = workSheetName;
            _PropertiesToSerialize = propertiesToSerialize;
        }

      

        internal void ChangeCellValue(string worksheetName, string cell, object value)
        {
            using (ExcelPackage excel = new ExcelPackage())
            {
                var worksheet = excel.Workbook.Worksheets[worksheetName];
                worksheet.Cells[cell].Value = value;
                excel.Save();
            }
        }



        internal void Write(ICollection<object[]> listElements,  string worksheetName,string filename)
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = ExcelManager.getWorksheet(excel, worksheetName);
                addHeader(excel, worksheetName, _PropertiesToSerialize);
                worksheet.Cells[2, 1].LoadFromArrays(listElements);
                excel.Save();
            }
        }


    

        private void addHeader(ExcelPackage excel, string worksheetName, StringList header)
        {
            List<string[]> headerRow = new List<string[]>();
            headerRow.Add(header.ToArray());

            // Determine the header range (e.g. A1:D1)
            string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

            // Target a worksheet
            var worksheet = ExcelManager.getWorksheet(excel,worksheetName);

            // Popular header row data
            worksheet.Cells[headerRange].LoadFromArrays(headerRow);
            worksheet.Cells[headerRange].AutoFilter = true;
        }


        public override void Append<Y>(T element, string path)
        {
            ExcelReader<T> reader = new ExcelReader<T>(_WorkSheetName);
            Y list = (Y)reader.read<Y>(path);
            list.Add(element);
            Write(list, path);
        }

        public override void Append<Y>(ListSerializable<T> listElements, string path, string ElementsName = "")
        {
            ExcelReader<T> reader = new ExcelReader<T>(_WorkSheetName);
            ListSerializable<T> list = reader.read<Y>(path);
            list.AddRange(listElements);
            Write(list, path);
        }

        public override void Write(T element, string path)
        {
            ExcelSerializer<T> serializer = new ExcelSerializer<T>(_PropertiesToSerialize);
            object[] objectSerialize = serializer.Serialize(element);
            ICollection<object[]> objects = new List<object[]>() { objectSerialize };
            Write(objects, _WorkSheetName, path);

        }

        public override void Write<Y>(Y listElements, string path)
        {
            ExcelSerializer<T> serializer = new ExcelSerializer<T>(_PropertiesToSerialize);
            ICollection<object[]> objects = serializer.SerializeList(listElements);
            Write(objects, _WorkSheetName,path);
        }
    }
}
