using FileManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Utils.FileManagement;
using Utils.FileReaderWriter.Reader.XML;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Serialization.StandardSerializer;

namespace Utils.FileReaderWriter.Standard
{
    public class XmlWriter<T>: AbstractWriter<T>,IIoFile<XDocument>, IWriter<T> where T : Serializable
    {

        private string _Path;

        private XmlSerializer<T> _XmlSerializer
        {
            get
            {
                return ConvertSerializer<XmlSerializer<T>>();
            }
        }

        public XmlWriter()
        {
            _serializer = new XmlSerializer<T>();
            setBinding(BindingFlags.Public | BindingFlags.Instance); // by default
        }

        public override void Append<Y>(T element, string path)
        {
            StandardWriter<T>.Append<Y>(_XmlSerializer, element, path);
        }

        public override void Append<Y>(ListSerializable<T> listElements, string path, string ElementsName = "")
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path + " not found");
            }

            if (string.IsNullOrEmpty(ElementsName))
            {
                throw new ArgumentNullException( nameof(ElementsName) + " is null or empty");
            }

            _Path = path;

            string text = _XmlSerializer.SerializeList<Y>(listElements);

            ///get just necesserary element
            XDocument docTemp = XDocument.Parse(text);
            XElement parent = docTemp.Descendants(ElementsName).First();
            List<XElement> elementToWrite = parent.Elements().ToList();

            ///read file
            Loader loaderXml = new Loader();
            string message = loaderXml.LoadXMLFile(path);
            if (string.IsNullOrEmpty(message))
            {
                XDocument doc = loaderXml.GetDocument();
                XElement element = doc.Element(ElementsName);
                element.Add(elementToWrite);
                Save(doc);
            }
        }

        public override void Write(T element, string path)
        {
            StandardWriter<T>.Write(_XmlSerializer, element, path);
        }

        public override void Write<Y>(Y listElements, string path)
        {
            StandardWriter<T>.Write<Y>(_XmlSerializer, listElements, path);
        }

        public void Save(XDocument instanceToSave)
        {
            instanceToSave.Save(_Path);
        }

        public void Open(string fileName)
        {
            FileManager.OpenFile(fileName);
        }
    }
}
