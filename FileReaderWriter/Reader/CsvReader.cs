using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Serialization.StandardSerializer;

namespace Utils.FileReaderWriter.Reader
{
    public class CsvReader<T> : IReader<T> where T:Serializable
    {
        /// <summary>
        /// represents csv separator
        /// </summary>
        private readonly char Separator;

        /// <summary>
        /// allows to know if there is a header of a file
        /// </summary>
        private bool HasHeader
        {
            get
            {
                return Headers != null;
            }
        }

        /// <summary>
        /// list of each header in the file
        /// </summary>
        private readonly StringList Headers;

        /// <summary>
        /// constructor, init separtor character
        /// if headers = null then no headers in file else there is a header in file define by string in stringlist
        /// </summary>
        /// <param name="separator"></param>
        public CsvReader(char separator, StringList headers = null)
        {
            Separator = separator;
            Headers = headers;
        }

        private void CheckCorrectHeader(string line)
        {
            if (HasHeader)
            {
                StringList headers = new StringList(line.Split(Separator));
                if (!headers.Equals(Headers))
                {
                    throw new ArgumentException("Headers defines not equal to headers in file");
                }
            }
        }

        /// <summary>
        /// allows to knows if file has header and if the header parameter is equal to header in file
        /// </summary>
        /// <param name="header"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool FileHasHeader(StringList header,string fileName)
        {
             return header.Join(Separator) == FileReader.ReadLines(fileName).First();
        }


        /// <summary>
        /// read all lines
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Lines</returns>
        public StringList readLine(string fileName, bool withHeader = false)
        {
            StringList elements = new StringList();

            using (var reader = new StreamReader(fileName))
            {
                int count = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (count == 0 && HasHeader)
                    {
                        CheckCorrectHeader(line);
                        if (withHeader)
                        {
                            elements.Add(line);
                        }
                    }
                    else
                    {
                        elements.Add(line);
                    }
                    count++;
                }
            }
            return elements;
        }

        /// <summary>
        /// read line and create object T 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns>Liste of T, one by line</returns>
        public ListSerializable<T> read<Y>(string filePath) where Y : ListSerializable<T>
        {
            ListSerializable<T> elements = (Y)Activator.CreateInstance(typeof(Y));
            using (var reader = new StreamReader(filePath))
            {
                int count = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (count == 0 && HasHeader)
                    {
                        CheckCorrectHeader(line);
                    }
                    else
                    {
                        IStandardSerializer<T> serializer = new CsvSerializer<T>(Separator);
                        Serializable element = serializer.Deserialize(line);
                        elements.Add((T)element);

                    }
                    count++;
                }
                
            }

            return elements;

        }

        /// <summary>
        /// read all values by line
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>list of all values in all lines</returns>
        public List<StringList> read(string fileName, bool withHeader = false)
        {
            List<StringList> elements = new List<StringList>();

            using (var reader = new StreamReader(fileName))
            {
                int count = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (HasHeader && count == 0)
                    {
                        CheckCorrectHeader(line);
                        if (withHeader)
                        {
                            StringList listElements = new StringList(line.Split(';'));
                            elements.Add(listElements);
                        }
                    }
                    else
                    {
                        StringList listElements = new StringList(line.Split(';'));
                        elements.Add(listElements);
                    }
                    count++;

                }
            }
            return elements;

        }

        Y IGenericReader<T>.read<Y>(string filePath)
        {
            return (Y)read<Y>(filePath);
        }
    }
}
