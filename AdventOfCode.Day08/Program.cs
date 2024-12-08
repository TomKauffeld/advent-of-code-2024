using AdventOfCode.Core.Helpers;
using System.Collections.Concurrent;

namespace AdventOfCode.Day08
{
    internal static class Program
    {
        private static async Task<Tuple<Dictionary<char, List<Tuple<int, int>>>, Tuple<int, int>>> GetInput(bool test = false)
        {
            List<string> data = await InputFileHelper.GetLines(08, test);
            Dictionary<char, List<Tuple<int, int>>> result = [];
            int height = data.Count;
            int width = 0;
            for (int y = 0; y < height; ++y)
            {
                if (data[y].Length > width)
                    width = data[y].Length;
                for (int x = 0; x < data[y].Length; ++x)
                {
                    char c = data[y][x];
                    if (c == '.')
                        continue;
                    if (result.ContainsKey(c))
                        result[c].Add(Tuple.Create(x, y));
                    else
                        result.Add(c, [Tuple.Create(x, y)]);
                }
            }

            return Tuple.Create(result, Tuple.Create(width, height));
        }

        private static async Task Part01()
        {
            (Dictionary<char, List<Tuple<int, int>>> data, Tuple<int, int> size) = await GetInput();
            ConcurrentBag<Tuple<int, int>> positions = [];

            Parallel.ForEach(data, item =>
            {
                for (int i = 0; i < item.Value.Count - 1; ++i)
                {
                    for (int j = i + 1; j < item.Value.Count; ++j)
                    {
                        Tuple<int, int> posA = item.Value[i];
                        Tuple<int, int> posB = item.Value[j];
                        Tuple<int, int> diff = Tuple.Create(
                            posA.Item1 - posB.Item1,
                            posA.Item2 - posB.Item2
                        );
                        Tuple<int, int> foundA = Tuple.Create(posA.Item1 + diff.Item1, posA.Item2 + diff.Item2);
                        Tuple<int, int> foundB = Tuple.Create(posB.Item1 - diff.Item1, posB.Item2 - diff.Item2);
                        if (foundA is { Item1: >= 0, Item2: >= 0 } && foundA.Item1 < size.Item1 && foundA.Item2 < size.Item2)
                            positions.Add(foundA);
                        if (foundB is { Item1: >= 0, Item2: >= 0 } && foundB.Item1 < size.Item1 && foundB.Item2 < size.Item2)
                            positions.Add(foundB);
                    }
                }
            });
            List<Tuple<int, int>> found = [];
            foreach (Tuple<int, int> position in positions)
            {
                if (!found.Contains(position))
                    found.Add(position);
            }
            Console.WriteLine($"Found {found.Count} positions");
        }

        private static async Task Part02()
        {
            (Dictionary<char, List<Tuple<int, int>>> data, Tuple<int, int> size) = await GetInput();
            ConcurrentBag<Tuple<int, int>> positions = [];

            Parallel.ForEach(data, item =>
            {
                for (int i = 0; i < item.Value.Count - 1; ++i)
                {
                    Tuple<int, int> posA = item.Value[i];
                    for (int j = i + 1; j < item.Value.Count; ++j)
                    {
                        Tuple<int, int> posB = item.Value[j];
                        Tuple<int, int> diff = Tuple.Create(
                            posA.Item1 - posB.Item1,
                            posA.Item2 - posB.Item2
                        );
                        Tuple<int, int> found = Tuple.Create(posA.Item1, posA.Item2);
                        int offset = 0;
                        while (found is { Item1: >= 0, Item2: >= 0 } && found.Item1 < size.Item1 && found.Item2 < size.Item2)
                        {
                            ++offset;
                            positions.Add(found);
                            found = Tuple.Create(
                                posA.Item1 + diff.Item1 * offset,
                                posA.Item2 + diff.Item2 * offset
                            );
                        }

                        offset = -1;
                        found = Tuple.Create(
                            posA.Item1 + diff.Item1 * offset,
                            posA.Item2 + diff.Item2 * offset
                        );

                        while (found is { Item1: >= 0, Item2: >= 0 } && found.Item1 < size.Item1 && found.Item2 < size.Item2)
                        {
                            --offset;
                            positions.Add(found);
                            found = Tuple.Create(
                                posA.Item1 + diff.Item1 * offset,
                                posA.Item2 + diff.Item2 * offset
                            );
                        }
                    }
                }
            });
            List<Tuple<int, int>> found = [];
            foreach (Tuple<int, int> position in positions)
            {
                if (!found.Contains(position))
                    found.Add(position);
            }
            Console.WriteLine($"Found {found.Count} positions");

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
