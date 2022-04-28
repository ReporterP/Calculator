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
        
        public string Pattern { get; set; }
        public string Description { get; set; }
        public Action Action { get; set; }

    }
}