using System;

namespace Calculator
{
    public class OneVarOperator
    {
        public OneVarOperator(string pattern, string description, Func<double, double> use)
        {
            this.Pattern = pattern;
            this.Description = description;
            this.Use = use;
        }
        public string Pattern { get; }
        public string Description { get; }
        public Func<double, double> Use { get; }
    }
}