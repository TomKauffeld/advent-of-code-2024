using AdventOfCode.Core.Helpers;
using System.Collections.Concurrent;

namespace AdventOfCode.Day09
{
    internal static class Program
    {
        private static async Task Part01()
        {
            string data = (await InputFileHelper.GetLines(09)).First();
            int length = data.Length;
            List<int> memory = [];
            for (int i = 0; i < length; ++i)
            {
                int d = data[i] - '0';
                if (i % 2 == 0)
                {
                    for (int j = 0; j < d; ++j)
                        memory.Add(i / 2);
                }
                else
                {
                    for (int j = 0; j < d; ++j)
                        memory.Add(-1);
                }
            }
            Console.WriteLine($"Created memory list: {memory.Count}");

            int last = 0;
            for (int i = memory.Count - 1; i >= 0; --i)
            {
                if (memory[i] > 0)
                {
                    int t = memory.IndexOf(-1, last + 1);
                    Console.WriteLine($"{i} - Found empty space at {t}");
                    if (t < 0)
                        break;
                    memory[t] = memory[i];
                    last = t;
                }
                memory.RemoveAt(i);
            }

            Console.WriteLine($"Compacted memory list: {memory.Count}");

            ConcurrentBag<long> numbers = [];

            Parallel.ForEach(memory, (v, _, i) =>
            {
                numbers.Add(i * v);
            });

            long sum = numbers.Sum();

            Console.WriteLine($"Found checksum: {sum}");
        }

        private static int IndexOfWithLength(this List<int> list, int value, int length, int offset)
        {
            int last = offset;

            while (true)
            {
                int index = list.IndexOf(value, last);
                if (index < 0)
                    return -1;
                if (index + length > list.Count)
                    return -1;
                bool ok = true;
                for (int i = index + 1; i < index + length; ++i)
                {
                    if (list[i] == value)
                        continue;
                    ok = false;
                    last = i;
                }
                if (ok)
                    return index;
            }
        }

        private static async Task Part02()
        {
            string data = (await InputFileHelper.GetLines(09)).First();
            int length = data.Length;
            List<int> memory = [];
            for (int i = 0; i < length; ++i)
            {
                int d = data[i] - '0';
                if (i % 2 == 0)
                {
                    for (int j = 0; j < d; ++j)
                        memory.Add(i / 2);
                }
                else
                {
                    for (int j = 0; j < d; ++j)
                        memory.Add(-1);
                }
            }
            Console.WriteLine($"Created memory list: {memory.Count}");

            for (int i = memory.Count - 1; i >= 0; --i)
            {
                if (memory[i] > 0)
                {
                    int j = i - 1;
                    while (memory[j] == memory[i])
                        --j;
                    ++j;
                    int l = i - j + 1;
                    i = j;
                    int t = memory.IndexOfWithLength(-1, l, 0);
                    if (t < 0)
                        continue;
                    if (t >= j)
                        continue;
                    Console.WriteLine($"{i} - Moving {memory[i]} (x {l}) to {t}");
                    for (int o = l - 1; o >= 0; --o)
                    {
                        memory[t + o] = memory[j];
                        memory[j + o] = -1;
                    }
                }
            }

            Console.WriteLine($"Compacted memory list: {memory.Count}");

            ConcurrentBag<long> numbers = [];

            Parallel.ForEach(memory, (v, _, i) =>
            {
                if (v > 0)
                    numbers.Add(i * v);
            });

            long sum = numbers.Sum();
            Console.WriteLine($"Found checksum: {sum}");
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
