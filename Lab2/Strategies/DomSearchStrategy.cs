using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Lab2.Models;

namespace Lab2.Strategies
{
    public class DomSearchStrategy : ISearchStrategy
    {
        public string Name
        {
            get { return "DOM API"; }
        }

        public List<Student> Search(string filepath, SearchCriteria criteria)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            List<Student> result = new List<Student>();

            List<string> conditions = new List<string>();
            foreach (var filter in criteria.Filters)
            {
                string attrName = filter.Key;
                string attrValue = filter.Value;
                conditions.Add("@" + attrName + "=\"" + attrValue + "\"");
            }

            string xpath = "//Student";
            if (conditions.Count > 0)
            {
                xpath += "[" + string.Join(" and ", conditions) + "]";
            }
    
            XmlNodeList? nodes = doc.SelectNodes(xpath);
            if (nodes == null)
            {
                return result;
            }

            foreach (XmlElement element in nodes)
            {
                if (criteria.Keyword != "")
                {
                    string keyword = criteria.Keyword.ToLower();
                    bool found = false;
                    
                    foreach (XmlAttribute attr in element.Attributes)
                    {
                        if (attr.Value.ToLower().Contains(keyword))
                        {
                            found = true;
                        }
                    }

                    XmlNodeList? keywordDisciplines = element.SelectNodes("Discipline");
                    if (keywordDisciplines != null)
                    {
                        foreach (XmlElement d in keywordDisciplines)
                        {
                            if (d.GetAttribute("Name").ToLower().Contains(keyword))
                            {
                                found = true;
                            }
                        }
                    }

                    if (!found)
                    {
                        continue;
                    }
                }

                Student student = new Student();
                
                foreach (XmlAttribute attr in element.Attributes)
                {
                    student.Attributes[attr.Name] = attr.Value;
                }

                XmlNodeList? disciplineNodes = element.SelectNodes("Discipline");
                if (disciplineNodes != null)
                {
                    foreach (XmlElement d in disciplineNodes)
                    {
                        string name = d.GetAttribute("Name");
                        int grade = int.Parse(d.GetAttribute("Grade"));
                        student.Disciplines.Add(new Discipline(name, grade));
                    }
                }

                result.Add(student);
            }

            return result;
        }
    }
}
