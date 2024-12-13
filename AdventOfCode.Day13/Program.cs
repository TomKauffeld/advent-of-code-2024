using AdventOfCode.Core.Helpers;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day13
{
    internal static partial class Program
    {
        private static async Task<List<ButtonPrice>> GetInput(bool test = false)
        {
            List<string> lines = await InputFileHelper.GetLines(13, test);
            List<ButtonPrice> prices = [];
            ButtonPrice price = new();
            foreach (string line in lines)
            {
                Match buttonMatch = ButtonRegex().Match(line);
                if (buttonMatch.Success)
                {
                    if (buttonMatch.Groups["button"].Value == "A")
                        price.ButtonA = (
                            long.Parse(buttonMatch.Groups["x"].Value),
                            long.Parse(buttonMatch.Groups["y"].Value)
                            );
                    else
                        price.ButtonB = (
                            long.Parse(buttonMatch.Groups["x"].Value),
                            long.Parse(buttonMatch.Groups["y"].Value)
                        );
                }
                else
                {
                    Match priceMatch = PriceRegex().Match(line);
                    if (priceMatch.Success)
                    {
                        price.Price = (
                            long.Parse(priceMatch.Groups["x"].Value),
                            long.Parse(priceMatch.Groups["y"].Value)
                        );
                        prices.Add(price);
                        price = new ButtonPrice();
                    }
                }
            }
            return prices;
        }


        private static async Task Part01()
        {
            const long maxButtonPresses = 100L;
            List<ButtonPrice> data = await GetInput();
            ConcurrentBag<(long a, long b, long t)> results = [];
            Parallel.ForEach(data, item =>
            {
                List<(long a, long b)> possibles = [];
                for (long i = 0L; i < maxButtonPresses; ++i)
                {
                    long x = item.ButtonA.x * i;
                    long y = item.ButtonA.y * i;
                    if (x > item.Price.x || y > item.Price.y)
                        break;
                    long bx, by;
                    if (item.ButtonB is { x: > 0, y: > 0 })
                    {
                        bx = (item.Price.x - x) / item.ButtonB.x;
                        by = (item.Price.y - y) / item.ButtonB.y;
                    }
                    else if (item.ButtonB is { x: > 0 })
                    {
                        bx = (item.Price.x - x) / item.ButtonB.x;
                        by = bx;
                    }
                    else if (item.ButtonB is { y: > 0 })
                    {
                        by = (item.Price.y - y) / item.ButtonB.y;
                        bx = by;
                    }
                    else if (x == item.Price.x && y == item.Price.y)
                    {
                        possibles.Add((i, 0));
                        continue;
                    }
                    else
                    {
                        continue;
                    }

                    if (bx > maxButtonPresses || bx != by)
                        continue;
                    x += bx * item.ButtonB.x;
                    y += by * item.ButtonB.y;
                    if (x == item.Price.x && y == item.Price.y)
                        possibles.Add((i, bx));
                }

                if (possibles.Count > 0)
                {
                    (long a, long b, long t) result = possibles
                        .Select(possible => (possible.a, possible.b, possible.a * 3L + possible.b))
                        .OrderBy(possible => possible.Item3)
                        .First();
                    results.Add(result);
                }
            });
            Console.WriteLine($"Found {results.Count} possible prices. Total cost {results.Sum(i => i.t)}");
        }

        private static async Task Part02()
        {
            const long offset = 10000000000000L;
            List<ButtonPrice> data = await GetInput();
            ConcurrentBag<(long a, long b, long t)> results = [];
            foreach (ButtonPrice item in data)
            {
                long px = item.Price.x + offset;
                long py = item.Price.y + offset;

                long ax = item.ButtonA.x;
                long ay = item.ButtonA.y;

                long bx = item.ButtonB.x;
                long by = item.ButtonB.y;

                long a = (px * by - py * bx) / (ax * by - ay * bx);
                long b = (py - a * ay) / by;

                long tx = a * ax + b * bx;
                long ty = a * ay + b * by;

                if (tx == px && ty == py)
                    results.Add((a, b, a * 3 + b));
            }
            Console.WriteLine($"Found {results.Count} possible prices. Total cost {results.Sum(i => i.t)}");
        }

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Part 01");
            await Part01();
            Console.WriteLine("Part 02");
            await Part02();
            Console.ReadLine();
        }

        [GeneratedRegex("^Button (?<button>[A-Z]): X\\+(?<x>[0-9]+), Y\\+(?<y>[0-9]+)$")]
        private static partial Regex ButtonRegex();



        [GeneratedRegex("^Prize: X=(?<x>[0-9]+), Y=(?<y>[0-9]+)$")]
        private static partial Regex PriceRegex();
    }
}
