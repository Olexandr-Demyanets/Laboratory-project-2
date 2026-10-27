using System;
using System.Collections.Generic;
using System.Text;
using Lab2.Models;

namespace Lab2.Strategies
{
    public interface ISearchStrategy
    {
        string Name { get; }

        List<Student> Search(string filepath, SearchCriteria criteria);
    }
}
