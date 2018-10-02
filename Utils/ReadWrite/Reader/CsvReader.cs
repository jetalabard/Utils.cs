using System;
using System.Collections.Generic;
using System.IO;
using Utils.Reader;
using Utils.ReadWrite.Serializer;
using Utils.Serializer;

namespace Utils.Reader
{
    public class CsvReader : IReader
    {
        /// <summary>
        /// represents csv separator
        /// </summary>
        private char Separator;

        /// <summary>
        /// allows to know if there is a header of a file
        /// </summary>
        private bool HasHeader;

        /// <summary>
        /// list of each header in the file
        /// </summary>
        private StringList Headers;

        /// <summary>
        /// constructor, init separtor character
        /// </summary>
        /// <param name="separator"></param>
        public CsvReader(char separator, bool header = false,StringList headers = null)
        {
            Separator = separator;
            HasHeader = header;
            if (HasHeader)
            {
                Headers = headers;
            }
        }

        private void ManageHeader(string line)
        {
            StringList headers = new StringList(line.Split(Separator));
            if(!headers.Equals(Headers))
            {
                throw new Exception("Headers defines not equal to headers in file");
            }
        }


        /// <summary>
        /// read all lines
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Lines</returns>
        public StringList readLine(string fileName)
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
                        ManageHeader(line);
                    }
                    else
                    {
                        elements.Add(line);
                    }
                    count++;
                }
                TestResultSize(count, elements);
            }
            return elements;
        }

        /// <summary>
        /// read line and create object T 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns>Liste of T, one by line</returns>
        public List<Serializable> read<T>(string fileName) where T: Serializable
        {
            List<Serializable> elements = new List<Serializable>();
            if (typeof(T).BaseType == typeof(Serializable))
            {
                using (var reader = new StreamReader(fileName))
                {
                    int count = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (count == 0 && HasHeader)
                        {
                            ManageHeader(line);
                        }
                        else
                        {
                            ISerializer serializer = new CsvSerializer(Separator);
                            T element = serializer.Deserialize<T>(line);
                            elements.Add(element);
                            
                        }
                        count++;
                    }

                    TestResultSize(count, elements);
                }
            }
            else
            {
                throw new Exception("Invalid Type: is not a readable object");
            }
            return elements;

        }

        /// <summary>
        /// read all values by line
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>list of all values in all lines</returns>
        public List<StringList> read(string fileName)
        {
            List<StringList> elements = new List<StringList>();

            using (var reader = new StreamReader(fileName))
            {
                int count = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (count == 0 && HasHeader)
                    {
                        ManageHeader(line);
                    }
                    else
                    {
                        StringList listElements = new StringList(line.Split(';'));
                        elements.Add(listElements);
                        
                    }
                    count++;

                }
                TestResultSize(count, elements);
            }
            return elements;

        }

        /// <summary>
        /// compare numbers of rows read and size of list
        /// </summary>
        /// <param name="count"></param>
        /// <param name="elements"></param>
        public void TestResultSize<T>(int count,List<T> elements)
        {
            int numberOfRows = HasHeader ? count - 1 : count;
            if (numberOfRows != elements.Count)
            {
                throw new Exception("Error during read file : ");
            }
        }
    }
}
