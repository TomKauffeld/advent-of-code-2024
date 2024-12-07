using AdventOfCode.Core.Helpers;
using System.Collections.Concurrent;

namespace AdventOfCode.Day07
{
    internal static class Program
    {
        public const int OperatorMultiplication = 0;
        public const int OperatorAdd = 1;
        public const int OperatorConcat = 2;

        private static bool IsSolution(long target, List<long> items, int operations)
        {
            long sum = items.First();
            for (int i = 0; i < items.Count - 1; ++i)
            {
                int operation = (operations >> i) & 1;
                switch (operation)
                {
                    case OperatorAdd:
                        sum += items[i + 1];
                        break;
                    case OperatorMultiplication:
                        sum *= items[i + 1];
                        break;
                    default:
                        throw new Exception("Not possible");
                }

                if (sum > target)
                    return false;
            }

            return sum == target;
        }

        private static List<int> FindSolutions(long target, List<long> items)
        {
            List<int> possible = [];
            int amountOfOperators = items.Count - 1;
            int amountOfPossible = (int)Math.Pow(2, amountOfOperators);
            for (int i = 0; i < amountOfPossible; ++i)
                possible.Add(i);
            return possible
                .AsParallel()
                .Where(i => IsSolution(target, items, i))
                .ToList();
        }

        private static async Task Part01()
        {
            List<Tuple<long, List<long>>> lists = await InputFileHelper.GetNumberToNumbersList(07);
            ConcurrentBag<long> solutions = [];
            Parallel.ForEach(lists, list =>
            {
                List<int> possibleOperations = FindSolutions(list.Item1, list.Item2);
                if (possibleOperations.Count > 0)
                    solutions.Add(list.Item1);
            });
            long sum = solutions.Sum();
            Console.WriteLine($"Found {solutions.Count} items, with the sum {sum}");
        }

        private static bool IsSolution2(long target, List<long> items, int operations)
        {
            long sum = items.First();
            for (int i = 0; i < items.Count - 1; ++i)
            {
                int operation = (operations >> (2 * i)) & 0b11;
                switch (operation)
                {
                    case OperatorAdd:
                        sum += items[i + 1];
                        break;
                    case OperatorMultiplication:
                        sum *= items[i + 1];
                        break;
                    case OperatorConcat:
                        sum = long.Parse($"{sum}{items[i + 1]}");
                        break;
                    default:
                        return false;
                }

                if (sum > target)
                    return false;
            }

            return sum == target;
        }

        private static List<int> FindSolutions2(long target, List<long> items)
        {
            List<int> possible = [];
            int amountOfOperators = items.Count - 1;
            int amountOfPossible = (int)Math.Pow(4, amountOfOperators);

            for (int i = 0; i < amountOfPossible; ++i)
                possible.Add(i);

            return possible
                .AsParallel()
                .Where(i => IsSolution2(target, items, i))
                .ToList();
        }

        private static async Task Part02()
        {
            List<Tuple<long, List<long>>> lists = await InputFileHelper.GetNumberToNumbersList(07);
            ConcurrentBag<long> solutions = [];
            Parallel.ForEach(lists, list =>
            {
                List<int> possibleOperations = FindSolutions2(list.Item1, list.Item2);
                if (possibleOperations.Count > 0)
                    solutions.Add(list.Item1);
            });
            long sum = solutions.Sum();
            Console.WriteLine($"Found {solutions.Count} items, with the sum {sum}");
        }

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Part 01");
            await Part01();
            Console.WriteLine("Part 02");
            await Part02();
            Console.ReadLine();
        }
    }
}