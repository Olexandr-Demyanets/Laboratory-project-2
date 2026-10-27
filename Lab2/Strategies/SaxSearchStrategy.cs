using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Xml;
using Lab2.Models;

namespace Lab2.Strategies
{
    public class SaxSearchStrategy : ISearchStrategy
    {
        public string Name
        {
            get { return "SAX API"; }
        }

        public List<Student> Search(string filePath, SearchCriteria criteria)
        {
            List<Student> result = new List<Student>();
            Student? current = null;

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Student")
                    {
                        current = new Student();
                        bool isEmpty = reader.IsEmptyElement;

                        while (reader.MoveToNextAttribute())
                        {
                            current.Attributes[reader.Name] = reader.Value;
                        }

                        if (isEmpty)
                        {
                            if (Matches(current, criteria))
                            {
                                result.Add(current);
                            }
                            current = null;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Discipline" && current != null)
                    {
                        string name = reader.GetAttribute("Name") ?? "";
                        int grade = int.Parse(reader.GetAttribute("Grade") ?? "0");
                        current.Disciplines.Add(new Discipline(name, grade));
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Student" && current != null)
                    {
                        if (Matches(current, criteria))
                        {
                            result.Add(current);
                        }
                        current = null;
                    }
                }
            }

            return result;
        }

        private bool Matches(Student student, SearchCriteria criteria)
        {
            foreach (var filter in criteria.Filters)
            {
                if (!student.Attributes.ContainsKey(filter.Key) || student.Attributes[filter.Key] != filter.Value)
                {
                    return false;
                }
            }

            if (criteria.Keyword != "")
            {
                string keyword = criteria.Keyword.ToLower();
                bool found = false;

                foreach (var pair in student.Attributes)
                {
                    if (pair.Value.ToLower().Contains(keyword))
                    {
                        found = true;
                    }
                }

                foreach (Discipline d in student.Disciplines)
                {
                    if (d.Name.ToLower().Contains(keyword))
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
