using System;
using System.Xml.Linq;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Serialization.StandardSerializer;

namespace Utils.FileReaderWriter.Reader.XML
{
    public class XmlReader<T> : IReader<T> where T : Serializable
    {

        private readonly XNamespace _namespaceUri;

        private readonly string _pathXsdFile;

        private readonly string _tagElements;

        private readonly string _typeElement;

        public XmlReader(string tagElements,string typeElement, XNamespace namespaceUri = null, string pathXsdFile = "")
        {
            _namespaceUri = namespaceUri;
            _pathXsdFile = pathXsdFile;
            _tagElements = tagElements;
            _typeElement = typeElement;
        }

        public bool ValidateSchema(string filePath)
        {
            Loader loader = new Loader();
            string errorMessage = loader.LoadXMLFile(filePath, _namespaceUri, _pathXsdFile);
            return string.IsNullOrEmpty(errorMessage);
        }


        public XDocument read(string filePath)
        {
            Loader loader = new Loader();
            string errorMessage = loader.LoadXMLFile(filePath, _namespaceUri, _pathXsdFile);
            if (string.IsNullOrEmpty(errorMessage))
            {
                return loader.GetDocument();
            }
            else
            {
                throw new InvalidOperationException(errorMessage);
            }
            
        }

        public ListSerializable<T> read<Y>(string filePath) where Y : ListSerializable<T>
        {
            Loader loader = new Loader();
            string errorMessage = loader.LoadXMLFile(filePath, _namespaceUri, _pathXsdFile);
            if (string.IsNullOrEmpty(errorMessage))
            {
                XDocument doc = loader.GetDocument();
                //test if file is a list of elements
                XElement element = doc.Element(_tagElements);
                IStandardSerializer<T> serializer = new XmlSerializer<T>();
                if (element != null)
                {
                    string result = string.Concat(element);
                    return serializer.DeserializeList<Y>(result);
                }
                else
                {
                    //test if file contains only one element
                    element = doc.Element(_typeElement);
                    if(element != null)
                    {
                        string result = string.Concat(element);
                        ListSerializable<T> elements = (Y)Activator.CreateInstance(typeof(Y));
                        elements.Add(serializer.Deserialize(result));
                        return elements;
                    }
                    else
                    {
                        throw new ArgumentException("File not contains this type of serializable object");
                    }
                    
                }

            }
            else
            {
                throw new InvalidOperationException(errorMessage);
            }
        }

        Y IGenericReader<T>.read<Y>(string filePath)
        {
            return (Y)read<Y>(filePath);
        }

    }
}
