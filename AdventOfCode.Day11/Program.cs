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

        private static long GetStonesAfter(string item, long depth, IDictionary<string, long> cache)
        {
            long result;
            if (depth <= 0L)
                result = 1L;
            else if (cache.TryGetValue($"{depth}:{item}", out result))
                return result;
            else if (item == "0")
                result = GetStonesAfter("1", depth - 1L, cache);
            else if (item.Length % 2 == 0)
            {
                result = GetStonesAfter(item[..(item.Length / 2)], depth - 1L, cache);
                result += GetStonesAfter(item[(item.Length / 2)..].TrimStart('0').PadLeft(1, '0'), depth - 1L, cache);
            }
            else
            {
                long number = long.Parse(item);
                result = GetStonesAfter($"{number * 2024}", depth - 1L, cache);
            }

            cache[$"{depth}:{item}"] = result;

            return result;
        }


        private static async Task Part02()
        {
            List<string> data = await GetInput();
            const long blinks = 75L;
            ConcurrentDictionary<string, long> cache = [];

            List<long> results = data.AsParallel().Select(
                item =>
                {
                    long items = GetStonesAfter(item, blinks, cache);
                    Console.WriteLine($"Found {items} stones after {blinks} blinks for {item}");
                    return items;
                }).ToList();
            Console.WriteLine($"Found {results.Sum()} stones after {blinks} blinks");
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
