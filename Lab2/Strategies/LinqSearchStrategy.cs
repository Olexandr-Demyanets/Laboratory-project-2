using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Lab2.Models;

namespace Lab2.Strategies
{
    public class LinqSearchStrategy : ISearchStrategy
    {
        public string Name
        {
            get { return "LINQ to XML"; }
        }

        public List<Student> Search(string filepath, SearchCriteria criteria)
        {
            XDocument doc = XDocument.Load(filepath);

            IEnumerable<XElement> query = doc.Descendants("Student");

            foreach (var filter in criteria.Filters)
            {
                string attrName = filter.Key;
                string attrValue = filter.Value;
                query = query.Where(s => (string?)s.Attribute(attrName) == attrValue);
            }

            if (criteria.Keyword != "")
            {
                string keyword = criteria.Keyword.ToLower();
                query = query.Where(s =>
                    s.Attributes().Any(a => a.Value.ToLower().Contains(keyword)) ||
                    s.Elements("Discipline").Any(d => ((string?)d.Attribute("Name") ?? "").ToLower().Contains(keyword)));
            }

            List<Student> result = new List<Student>();

            foreach (XElement element in query)
            {
                Student student = new Student();

                foreach (XAttribute attr in element.Attributes())
                {
                    student.Attributes[attr.Name.LocalName] = attr.Value;
                }

                foreach(XElement d in element.Elements("Discipline"))
                {
                    string name = (string?)d.Attribute("Name") ?? "";
                    int grade = int.Parse((string?)d.Attribute("Grade") ?? "0");
                    student.Disciplines.Add(new Discipline(name, grade));
                }

                result.Add(student);
            }

            return result;
        }
    }
    
}