using AdventOfCode.Core.Helpers;
using System.Collections.Concurrent;

namespace AdventOfCode.Day11
{
    internal static class Program
    {
        private static async Task<List<string>> GetInput(bool test = false)
        {
            return (await InputFileHelper.GetSpaceSeparatedNumbers(11, test))
                .First()
                .Select(x => x.ToString())
                .ToList();
        }


        private static List<string> BlinkPart01(string data)
        {
            if (data == "0")
                return ["1"];
            if (data.Length % 2 == 0)
                return [
                    data[..(data.Length / 2)],
                    data[(data.Length / 2)..].TrimStart('0').PadLeft(1, '0')
                ];
            long number = long.Parse(data);
            return [$"{number * 2024}"];
        }

        private static List<string> BlinkPart01(List<string> data)
        {
            ConcurrentBag<string> results = [];

            Parallel.ForEach(data, item =>
            {
                foreach (string r in BlinkPart01(item))
                    results.Add(r);
            });

            return results.ToList();
        }

        private static ConcurrentBag<string> BlinkPart02(ConcurrentBag<string> data)
        {
            ConcurrentBag<string> results = [];

            Parallel.ForEach(data, item =>
            {
                foreach (string r in BlinkPart01(item))
                    results.Add(r);
            });

            return results;
        }

        private static async Task Part01()
        {
            List<string> data = await GetInput();
            const int blinks = 25;

            Console.WriteLine($"0: {data.Count}");

            for (int i = 0; i < blinks; ++i)
            {
                data = BlinkPart01(data);
                Console.WriteLine($"{i + 1}: {data.Count}");
            }

            Console.WriteLine($"Found {data.Count} stones after {blinks} blinks");
        }

        private static async Task Part02()
        {
            ConcurrentBag<string> data = [.. await GetInput(true)];
            const int blinks = 75;

            Console.WriteLine($"0: {data.Count}");

            for (int i = 0; i < blinks; ++i)
            {
                data = BlinkPart02(data);
                Console.WriteLine($"{i + 1}: {data.Count}");
            }

            Console.WriteLine($"Found {data.Count} stones after {blinks} blinks");
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
