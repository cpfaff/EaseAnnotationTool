
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using CAFE.Core.Integration;
using CAFE.Core.Misc;

namespace CAFE.Services.Integration
{
    /// <summary>
    /// Service for import/export annotation item to and from other xml data structure
    /// </summary>
    public class XmlAnnotationItemIntegrationService : IAnnotationItemIntegrationService
    {
        /// <summary>
        /// Imports data structure as string and transform with stylesheet(optional)
        /// </summary>
        /// <param name="source">Data structure to import</param>
        /// <param name="stylesheet">Transformation stylesheet</param>
        /// <returns>Imported annotation item</returns>
        public AnnotationItem ImportWithTransform(string source, string stylesheet = null)
        {
            var inputData = source;
            var result = default(AnnotationItem);

            //First check that transformation needed
            if (stylesheet.IsNotNull())
            {
                try
                {
                    inputData = TransformData(source, stylesheet);
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Can't read stylesheet data. Maybe data stylesheet wrong");
                }
            }

            //And try to deserialize annotation item
            try
            {
                result = DeserializeData(inputData);
            }
            catch (Exception exception)
            {
                throw new DeserializeSchemaException("Can't import Annotation item. Input format is wrong");
            }

            //and need to check that result is not null, it's important
            if (result.IsNull()) throw new InvalidOperationException("Imported data is empty");

            return result;
        }

        private string TransformData(string source, string stylesheet)
        {
            //init compiled transform class
            var tranformator = new XslCompiledTransform();

            //create xml writer from string and load to transform
            using (var treader = new StringReader(stylesheet))
            {
                using (var xreader = XmlReader.Create(treader))
                {
                    tranformator.Load(xreader);
                }
            }

            //prepare result container for transformed data
            var result = new StringBuilder();

            //load xml string to inmemmory xml model
            var sourceDocument = new XmlDocument();
            sourceDocument.LoadXml(source);

            //open writer for result and perform transformation
            using (var twriter = new StringWriter(result))
            {
                using (var xwriter = new XmlTextWriter(twriter))
                {
                    tranformator.Transform(sourceDocument, xwriter);
                }
            }

            return result.ToString();
        }

        private AnnotationItem DeserializeData(string inputData)
        {
            //init xml data serializer
            XmlSerializer serializer = new XmlSerializer(typeof(AnnotationItem));

            XmlReaderSettings settings = new XmlReaderSettings();
            // No settings need modifying here

            var result = default(AnnotationItem);

            //create xml writer from string
            using (var treader = new StringReader(inputData))
            {
                using (var xreader = XmlReader.Create(treader, settings))
                {
                    if (!serializer.CanDeserialize(xreader)) throw new InvalidOperationException("Can't deserialize");

                    //perform desealization
                    object obj = serializer.Deserialize(xreader);
                    result = obj as AnnotationItem;
                }
            }

            return result;
        }


        /// <summary>
        /// Exports annotation item to string
        /// </summary>
        /// <param name="annotationItem">Annotation item instance</param>
        /// <returns>Prepared annotation item data structure</returns>
        public string Export(AnnotationItem annotationItem, string host = "")
        {
            //init xml data serializer
            XmlSerializer serializer = new XmlSerializer(typeof(AnnotationItem));

            var sb = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings();
            //settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
            settings.Encoding = Encoding.UTF8;
            //settings.Indent = false;
            //settings.OmitXmlDeclaration = false;


            foreach(var res in annotationItem.Object.Resources)
            {
                foreach(var onlineRes in res.OnlineResources)
                {
                    if(!onlineRes.DownloadUrl.EndsWith("/"))
                        onlineRes.DownloadUrl = host + "/" + onlineRes.DownloadUrl;
                    else
                        onlineRes.DownloadUrl = host + onlineRes.DownloadUrl;


                    if (!onlineRes.ReferenceUrl.EndsWith("/"))
                        onlineRes.ReferenceUrl = host + "/" + onlineRes.ReferenceUrl;
                    else
                        onlineRes.ReferenceUrl = host + onlineRes.ReferenceUrl;

                }
            }


            using (StringWriter textWriter = new StringWriter(sb))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, annotationItem);
                }
            }

            return sb.ToString();
        }
    }

    public class DeserializeSchemaException : ApplicationException
    {
        public DeserializeSchemaException() : base()
        {

        }

        public DeserializeSchemaException(string message) : base(message)
        {

        }
    }
}
