using System.Collections.Generic;
using Lab2.Models;
using Lab2.Strategies;

namespace Lab2.Services
{
    public class SearchContext
    {
        private ISearchStrategy strategy;

        public SearchContext(ISearchStrategy strategy)
        {
            this.strategy = strategy;
        }

        public string StrategyName
        {
            get { return strategy.Name; }
        }

        public void SetStrategy(ISearchStrategy strategy)
        {
            this.strategy = strategy;
        }

        public List<Student> Search(string filePath, SearchCriteria criteria)
        {
            return strategy.Search(filePath, criteria);
        }
    }
}
