using System;
using System.Collections.Generic;

namespace Calculator
{
    internal static class Program
    {
        private static readonly List<Command> Commands = new List<Command>()
        {
            new Command(pattern: "c", description: "Отчистить", action: Console.Clear),
            new Command(pattern: "h", description: "Помощь", action: ShowHelp),
            new Command(pattern: "q", description: "Закрыть", action: () => Environment.Exit(0)),

        };

        private static void UseCommand(string commandPattern)
        {
            foreach (var command in Commands)
            {
                if (commandPattern == command.Pattern)
                {
                    command.Action();
                    return;
                }
            }

            // throw "Нет такой команды"; // TODO: #!
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Помощь:");
            foreach (var command in Commands)
            {
                Console.WriteLine($"\\{command.Pattern} - {command.Description}");
            }
        }
        
        
        public static void Main(string[] args)
        {
            ShowHelp();
            while (true)
            {   
                Console.Write("$");
                string inputLine = Console.ReadLine()!;
                if (inputLine.StartsWith("\\"))
                {
                    UseCommand(inputLine.TrimStart('\\'));
                }
            }
        }
    }
}