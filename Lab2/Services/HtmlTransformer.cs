using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace Lab2.Services
{
    public class HtmlTransformer
    {
        public void Transform(string xmlPath, Stream xslStream, string htmlPath)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();

            using (XmlReader xslReader = XmlReader.Create(xslStream))
            {
                xslt.Load(xslReader);
            }

            xslt.Transform(xmlPath, htmlPath);
        }
    }
}