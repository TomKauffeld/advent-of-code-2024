using AdventOfCode.Core.Helpers;
using System.Collections.Concurrent;

namespace AdventOfCode.Day05
{
    internal static class Program
    {
        private static bool RespectsRule(List<int> numbers, List<int> rule)
        {
            if (rule.Count < 1)
                return true;
            int lastIndex = numbers.IndexOf(rule.First());
            if (lastIndex < 0)
                return true;
            for (int i = 1; i < rule.Count; ++i)
            {
                int index = numbers.IndexOf(rule[i]);
                if (index < 0)
                    return true;
                if (index < lastIndex)
                    return false;

                lastIndex = index;
            }

            return true;
        }

        private static bool RespectsRules(List<int> numbers, List<List<int>> rules)
        {
            return rules.AsParallel().All(rule => RespectsRule(numbers, rule));
        }

        private static async Task Part01()
        {
            (List<List<int>> rules, List<List<int>> jobs) = await InputFileHelper.GetPipeToCommaSeparatedNumbers(05);

            ConcurrentBag<int> numbers = [];
            Parallel.ForEach(jobs, job =>
            {
                List<List<int>> applicableRules = rules.Where(items => items.All(job.Contains)).ToList();
                bool respectsRules = RespectsRules(job, applicableRules);
                if (respectsRules)
                {
                    int index = job.Count / 2;
                    numbers.Add(job[index]);
                }
            });
            int sum = numbers.Sum();
            Console.WriteLine($"Found {sum}");
        }

        private static int CorrectRule(List<int> numbers, List<int> rule)
        {
            if (rule.Count < 1)
                return 0;
            bool hasCorrected = true;
            int corrected = 0;
            while (hasCorrected)
            {
                hasCorrected = false;

                int lastIndex = numbers.IndexOf(rule.First());
                if (lastIndex < 0)
                    return 0;

                for (int i = 1; i < rule.Count; ++i)
                {
                    int index = numbers.IndexOf(rule[i]);
                    if (index < 0)
                        return 0;


                    if (index < lastIndex)
                    {
                        (numbers[index], numbers[lastIndex]) = (numbers[lastIndex], numbers[index]);
                        hasCorrected = true;
                        ++corrected;
                        break;
                    }

                    lastIndex = index;
                }
            }

            return corrected;
        }

        private static int CorrectRules(List<int> numbers, List<List<int>> rules)
        {
            bool hasCorrected = true;
            int corrected = 0;
            while (hasCorrected)
            {
                hasCorrected = false;
                foreach (List<int> rule in rules)
                {
                    int tmp = CorrectRule(numbers, rule);
                    if (tmp > 0)
                    {
                        corrected += tmp;
                        hasCorrected = true;
                        break;
                    }
                }
            }

            return corrected;
        }


        private static async Task Part02()
        {
            (List<List<int>> rules, List<List<int>> jobs) = await InputFileHelper.GetPipeToCommaSeparatedNumbers(05);

            ConcurrentBag<int> numbers = [];
            Parallel.ForEach(jobs, job =>
            {
                List<List<int>> applicableRules = rules.Where(items => items.All(job.Contains)).ToList();
                bool respectsRules = RespectsRules(job, applicableRules);
                if (!respectsRules)
                {
                    CorrectRules(job, applicableRules);
                    int index = job.Count / 2;
                    numbers.Add(job[index]);
                }
            });
            int sum = numbers.Sum();
            Console.WriteLine($"Found {sum}");
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
