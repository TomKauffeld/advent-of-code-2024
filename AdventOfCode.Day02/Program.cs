using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day02
{
    internal static class Program
    {
        private static async Task Part01()
        {
            List<List<int>> data = await InputFileHelper.GetSpaceSeparatedNumbers(02);
            int result = data.AsParallel().Sum(items =>
            {
                bool increasing = items[1] > items[0];
                for (int i = 1; i < items.Count; ++i)
                {
                    if (increasing != items[i] > items[i - 1])
                        return 0;
                    int diff = Math.Abs(items[i] - items[i - 1]);
                    if (diff is < 1 or > 3)
                        return 0;
                }
                return 1;
            });
            Console.WriteLine($"Found {result} safe reports");
        }


        private static int GetFirstMismatchIndex(List<int> items)
        {
            bool increasing = items[1] > items[0];
            for (int i = 1; i < items.Count; ++i)
            {
                if (increasing != items[i] > items[i - 1])
                    return i - 1;
                int diff = Math.Abs(items[i] - items[i - 1]);
                if (diff is < 1 or > 3)
                    return i - 1;
            }
            return -1;
        }

        private static bool IsSafeWithOneTolerance(List<int> items)
        {
            int mismatch = GetFirstMismatchIndex(items);
            if (mismatch < 0)
                return true;
            List<int> retry = items.Where((_, i) => i != mismatch).ToList();
            if (GetFirstMismatchIndex(retry) < 0)
                return true;
            retry = items.Where((_, i) => i != mismatch + 1).ToList();
            if (GetFirstMismatchIndex(retry) < 0)
                return true;
            retry = items.Where((_, i) => i != mismatch - 1).ToList();
            return GetFirstMismatchIndex(retry) < 0;
        }

        private static async Task Part02()
        {
            List<List<int>> data = await InputFileHelper.GetSpaceSeparatedNumbers(02);
            int result = data.AsParallel().Sum(
                items => IsSafeWithOneTolerance(items) ? 1 : 0
            );
            Console.WriteLine($"Found {result} safe reports");
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
