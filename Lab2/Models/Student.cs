using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models
{
    public class Student
    {
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();
        public List<Discipline> Disciplines { get; set; } = new List<Discipline>();

        public override string ToString()
        {
            string result = "";

            foreach (var pair in Attributes)
            {
                result += pair.Key + ": " + pair.Value + "\n";
            }

            result += "Дисципліни:\n";

            foreach (Discipline d in Disciplines)
            {
                result += "   " + d.Name + " - " + d.Grade + "\n";
            }

            return result;
        }
    }
}
