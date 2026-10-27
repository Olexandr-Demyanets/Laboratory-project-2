using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models
{
    public class SearchCriteria
    {
        public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
        public string Keyword { get; set; } = "";
    }
}
