using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Calculator
{
    internal static class Program
    {
        private static readonly List<Command> Commands = new()
        {
            new Command("c", "Отчистить", Console.Clear),
            new Command("h", "Помощь", ShowHelp),
            new Command("q", "Закрыть", () => Environment.Exit(0))
        };

        private static readonly List<TwoVarOperatior> TwoVarOperators = new()
        {
            new TwoVarOperatior("**", "Степень", Math.Pow),
            new TwoVarOperatior("*", "Умножение", (d, d1) => d * d1),
            new TwoVarOperatior("/", "Деление", (d, d1) => d / d1),
            new TwoVarOperatior("+", "Сложение", (d, d1) => d + d1),
            new TwoVarOperatior("-", "Вычитание", (d, d1) => d - d1),
            new TwoVarOperatior("%", "Процент от числа", (d1, d2) => d1  / 100 * d2)
        };

        private static readonly List<OneVarOperator> OneVarOperators = new()
        {
            new OneVarOperator("//", "Квадратный корень", Math.Sqrt)
        };


        private static void UseCommand(string commandPattern)
        {
            foreach (var command in Commands.Where(command => commandPattern == command.Pattern))
            {
                command.Action();
                return;
            }

            throw new NotImplementedException("Нет такой команды!");
        }

        private static void InLogThis(string inputLine, double result)
        {
            using var file = File.AppendText("/Users/cybercat/log.txt");
            file.WriteLine($"{DateTime.Now} :: {inputLine} = {result}");
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Помощь:");
            foreach (var command in Commands)
                Console.WriteLine($"\\{command.Pattern} - {command.Description}");
            Console.WriteLine("Операторы:");
            foreach (var oneVarOperator in OneVarOperators)
                Console.WriteLine($"{oneVarOperator.Pattern} - {oneVarOperator.Description}");
            foreach (var twoVarOperator in TwoVarOperators)
                Console.WriteLine($"{twoVarOperator.Pattern} - {twoVarOperator.Description}");
            Console.WriteLine("Формат записи: 1 + 2 * 2 + //2 (символы и числительные через пробел)");
        }

        private static double Calculate(string inputline)
        {
            var inputData = inputline.Split(' ').ToList();
            for (var i = 0; i < inputData.Count; i++)
                foreach (var oneVarOperator in OneVarOperators
                             .Where(oneVarOperator => inputData[i].Contains(oneVarOperator.Pattern)))
                    inputData[i] =
                        $"{oneVarOperator.Use(double.Parse(inputData[i].Replace(oneVarOperator.Pattern, "")))}";

            TwoVarOperators:
            foreach (var twoVarOperatior in TwoVarOperators)
                for (var i = 0; i < inputData.Count; i++)
                {
                    if (twoVarOperatior.Pattern != inputData[i]) continue;
                    var result =
                        $"{twoVarOperatior.Use(double.Parse(inputData[i - 1]), double.Parse(inputData[i + 1]))}";
                    inputData.RemoveRange(i - 1, 3);
                    inputData.Insert(i - 1, result);
                }

            if (inputData.Count > 1)
            {
                foreach (var twoVarOperatior in TwoVarOperators.Where(twoVarOperatior =>
                             inputData.Select(e => twoVarOperatior.Pattern == e).Any())) goto TwoVarOperators;
                throw new FormatException("Не правильный формат");
            }

            double answer = double.Parse(inputData.First());
            InLogThis(inputline, answer);
            return answer;
        }

        public static void Main(string[] args)
        {
            ShowHelp();
            while (true)
                try
                {
                    Console.Write("$");
                    var inputLine = Console.ReadLine()!.Trim();
                    if (inputLine.StartsWith("\\"))
                        UseCommand(inputLine.TrimStart('\\'));
                    else
                        Console.WriteLine($"#{Calculate(inputLine)}");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Не правильный формат!");
                }
                catch (NotImplementedException)
                {
                    Console.WriteLine("Не знаем такую функцию!");
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Нет доступа к логам :(. Может вам поменять путь на 46 строке Program.cs?");
                }
                catch
                {
                    Console.WriteLine("Капец, ну что-то неотслеживаемое сломали...");
                }
        }
    }
}