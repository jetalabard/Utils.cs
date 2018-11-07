using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Utils.ReadWrite.Reader;
using Utils.ReadWrite.Serialization;
using Utils.ReadWrite.Serialization.StandardSerializer;

namespace Utils.ReadWrite.Writer.Standard
{
    public class XmlWriter<T>: AbstractWriter<T>, IWriter<T> where T : Serializable
    {

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
                doc.Save(path);
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

       
    }
}
