using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day01
{
    internal static class Program
    {
        private static async Task Part01()
        {
            Tuple<List<int>, List<int>> data = await InputFileHelper.GetDualNumbers(01);
            List<int> left = data.Item1.Order().ToList();
            List<int> right = data.Item2.Order().ToList();
            if (left.Count != right.Count)
            {
                Console.WriteLine("Invalid lengths");
                return;
            }

            int[] results = new int[left.Count];
            Parallel.For(0, left.Count, i =>
            {
                int d = Math.Abs(left[i] - right[i]);
                results[i] = d;
            });
            int result = results.Sum();
            Console.WriteLine($"Found sum: {result}");
        }

        private static async Task Part02()
        {
            Tuple<List<int>, List<int>> data = await InputFileHelper.GetDualNumbers(01);
            Dictionary<int, int> multipliers = new();
            Console.WriteLine("Creating Dictionary");
            foreach (int item in data.Item2)
                multipliers[item] = 1 + multipliers.GetValueOrDefault(item, 0);

            Console.WriteLine("Calculating Sum");
            int[] results = new int[data.Item1.Count];
            Parallel.For(0, data.Item1.Count, i =>
            {
                int d = data.Item1[i];
                int s = d * multipliers.GetValueOrDefault(d, 0);
                results[i] = s;
            });
            int result = results.Sum();
            Console.WriteLine($"Found sum: {result}");
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
