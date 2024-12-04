using AdventOfCode.Core.Helpers;
using System.Collections.Concurrent;

namespace AdventOfCode.Day04
{
    internal static class Program
    {
        private static string GetSubString(List<string> lines, int x, int y, int dx, int dy, int length)
        {
            string text = string.Empty;
            if (y + dy * (length - 1) >= lines.Count || y + dy * (length - 1) < 0)
                return string.Empty;
            for (int i = 0; i < length; ++i)
            {
                if (x >= lines[y].Length || x < 0)
                    return string.Empty;
                text += lines[y][x];
                x += dx;
                y += dy;
            }

            return text;
        }

        private static async Task Part01()
        {
            const string search = "XMAS";
            int length = search.Length;
            List<string> lines = await InputFileHelper.GetLines(04);
            ConcurrentBag<int> found = [];
            Parallel.For(0, lines.Count, y =>
            {
                string line = lines[y];
                Parallel.For(0, line.Length, x =>
                {
                    if (line[x] != 'X')
                        return;
                    string[] possibles =
                    [
                        GetSubString(lines, x, y, 1, 0, length),
                        GetSubString(lines, x, y, -1, 0, length),
                        GetSubString(lines, x, y, 0, 1, length),
                        GetSubString(lines, x, y, 0, -1, length),
                        GetSubString(lines, x, y, 1, 1, length),
                        GetSubString(lines, x, y, 1, -1, length),
                        GetSubString(lines, x, y, -1, 1, length),
                        GetSubString(lines, x, y, -1, -1, length),
                    ];
                    int sum = possibles.Count(s => s == search);
                    if (sum > 0)
                        found.Add(sum);
                });
            });
            int total = found.Sum();
            Console.WriteLine($"Found {search} a total of {total} times");
        }


        private static async Task Part02()
        {
            List<string> lines = await InputFileHelper.GetLines(04);
            ConcurrentBag<int> found = [];
            Parallel.For(1, lines.Count - 1, y =>
            {
                string line = lines[y];
                Parallel.For(1, line.Length - 1, x =>
                {
                    if (line[x] != 'A')
                        return;
                    string tl = $"{lines[y - 1][x - 1]}{lines[y + 1][x + 1]}";
                    string tr = $"{lines[y - 1][x + 1]}{lines[y + 1][x - 1]}";
                    if ((tl is "MS" or "SM") && (tr is "MS" or "SM"))
                    {
                        found.Add(1);
                    }
                });
            });
            int total = found.Sum();

            Console.WriteLine($"Found X-MAS {total} times");
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
