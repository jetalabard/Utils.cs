using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Utils.FileReaderWriter.Reader.XML
{
    internal class Loader
    {
        protected XDocument mXDoc;

        private string _ErrorMessage = "";

        /// <summary>
        /// loads an xml file
        /// </summary>
        /// <param name="xml_file">xml file to load</param>
        internal string LoadXMLFile(string xml_file, XNamespace namespaceURI = null, string xsdFile = "")
        {
            try
            {
                mXDoc = XDocument.Load(xml_file);
                if (!string.IsNullOrEmpty(xsdFile) && namespaceURI != null)
                {
                    XmlSchemaSet schemaSet = new XmlSchemaSet();
                    schemaSet.Add(namespaceURI.NamespaceName, xsdFile);
                    mXDoc.Validate(schemaSet, ValidatingProblemHandler);
                }
            }
            catch (FileNotFoundException e)
            {
                _ErrorMessage = "File Not Found: " + e.Message;
            }
            catch (Exception e)
            {
                _ErrorMessage = "ERROR: " + e.Message;
            }
            return _ErrorMessage;
        }

        /// <summary>
        /// Handler for schema validation of the score xml file
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">arguments of the event</param>
        private void ValidatingProblemHandler(object sender, ValidationEventArgs e)
        {
            _ErrorMessage = "ERROR: " + e.Message;
        }


        internal XDocument GetDocument()
        {
            return mXDoc;
        }
    }
}


