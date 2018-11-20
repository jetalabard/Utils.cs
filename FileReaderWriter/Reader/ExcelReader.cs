using LinqToExcel;
using System.Collections.Generic;
using System.Linq;
using Utils;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Serialization.SpecificSerializer;

namespace Utils.FileReaderWriter.Reader
{
    public class ExcelReader<T> : IReader<T> where T : Serializable
    {

        private readonly string _WorkSheetName;
                
        private readonly ISpecificSerializer<T> _Serializer;

        public ExcelReader(string worksheetName, StringList headers = null)
        {
            _WorkSheetName = worksheetName;
            _Serializer = new ExcelSerializer<T>(headers);
        }

        public ListSerializable<T> read<Y>(string filePath) where Y : ListSerializable<T>
        {
            var excel = new ExcelQueryFactory(filePath);
            var queryExcel = from item in excel.Worksheet(_WorkSheetName)
                             select item;
            return _Serializer.DeserializeList<Y>(new List<object>(queryExcel.ToList()));
        }


        Y IGenericReader<T>.read<Y>(string filePath)
        {
            return (Y)read<Y>(filePath);
        }
    }
}
