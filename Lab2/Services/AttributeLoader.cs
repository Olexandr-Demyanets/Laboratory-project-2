using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Lab2.Models;

namespace Lab2.Services
{
    public class AttributeLoader
    {
        public Dictionary<string, List<String>> LoadAttributeValues(string filepath)
        {
            XDocument doc = XDocument.Load(filepath);
            IEnumerable<XElement> query = doc.Descendants("Student");
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

            foreach (XElement student in query)
            {
                foreach (XAttribute attr in student.Attributes())
                {
                    string name = attr.Name.LocalName;
                    string value = attr.Value;
                    if (!result.ContainsKey(name))
                    {
                        result[name] = new List<string>();
                    }
                    if (!result[name].Contains(value))
                    {
                        result[name].Add(value);
                    }
                }
            }
            foreach (var pair in result)
            {
                pair.Value.Sort();
            }

            return result;
        }
    }
}
