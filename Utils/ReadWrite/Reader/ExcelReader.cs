using LinqToExcel;
using System;
using System.Linq;
using Utils.ReadWrite.Serialization;
using Utils.ReadWrite.Serialization.Reflection;

namespace Utils.ReadWrite.Reader
{
    public class ExcelReader<T> : IReader<T> where T : Serializable
    {

        private readonly string _WorkSheetName;

        private readonly StringList _Headers;

        public ExcelReader(string worksheetName,StringList headers = null)
        {
            _WorkSheetName = worksheetName;
            _Headers = headers;
        }

        public ListSerializable<T> read<Y>(string filePath) where Y : ListSerializable<T>
        {
            ListSerializable<T> elements = (Y)Activator.CreateInstance(typeof(Y));
            var excel = new ExcelQueryFactory(filePath);
            var queryExcel = from item in excel.Worksheet(_WorkSheetName)
                        select item;
            foreach (var item in queryExcel)
            {
                StringList element = new StringList();
                foreach (string s in item)
                {
                    element.Add(s);
                }
                T lineObject = CreateInstanceManager<T>.CreateInstance(element);
                lineObject.Construct();
                elements.Add(lineObject);
            }
            return elements;


        }

        Y IGenericReader<T>.read<Y>(string filePath)
        {
            return (Y)read<Y>(filePath);
        }
    }
}
