using AdventOfCode.Core.Helpers;
using System.Collections.Concurrent;

namespace AdventOfCode.Day17
{
    internal static class Program
    {
        private static async Task<ChronospatialVm> GetInput(bool test = false)
        {
            List<string> str = await InputFileHelper.GetLines(17, test);
            Dictionary<char, long> registers = [];
            List<byte> memory = [];
            foreach (string line in str)
            {
                if (line.StartsWith("Register "))
                {
                    char register = line["Register ".Length];
                    long value = long.Parse(line[("Register X: ".Length)..]);
                    registers.Add(register, value);
                }
                else if (line.StartsWith("Program: "))
                {
                    string[] items = line["Program: ".Length..].Split(',');
                    memory.AddRange(items.Select(byte.Parse));
                }
            }

            ChronospatialVm vm = new(memory.ToArray());
            foreach (KeyValuePair<char, long> register in registers)
                vm.SetRegister(register.Key, register.Value);
            return vm;
        }

        private static async Task Part01()
        {
            ChronospatialVm vm = await GetInput();
            IReadOnlyList<byte> results = vm.ExecuteAll();
            Console.WriteLine(string.Join(",", results));
        }

        private static bool Equals(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            if (a.Count != b.Count)
                return false;
            return !a.Where((t, i) => t != b[i]).Any();
        }

        private static async Task Part02()
        {
            const int perTry = 10000000;
            ChronospatialVm vm = await GetInput();
            IReadOnlyList<byte> memory = vm.GetMemory();
            ConcurrentBag<long> values = [];
            int offset = 0;
            while (values.IsEmpty)
            {
                Console.WriteLine($"Try from {offset} to {offset + perTry}");
                Parallel.For(offset, offset + perTry, i =>
                {
                    ChronospatialVm tmpVm = vm.Clone();
                    tmpVm.SetRegister('A', i);
                    IReadOnlyList<byte> results = tmpVm.ExecuteAll();
                    if (Equals(memory, results))
                        values.Add(i);
                });
                offset += perTry;
            }
            long? min = values.Min();

            Console.WriteLine($"Found {values.Count} solution, min: {min}");
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
