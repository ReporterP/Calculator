using System;

namespace Calculator
{
    public class Command
    {
        public Command(string pattern, string description, Action action)
        {
            this.Action = action;
            this.Description = description;
            this.Pattern = pattern;
        }
        
        public string Pattern { get; }
        public string Description { get; }
        public Action Action { get; }

    }
}