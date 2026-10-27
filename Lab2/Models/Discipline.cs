using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models
{
    public class Discipline
    {
        public string Name { get; set; }
        public int Grade { get; set; }

        public Discipline(string name, int grade)
        {
            Name = name;
            Grade = grade;
        }
    }
}
