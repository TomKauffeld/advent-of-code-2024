using AdventOfCode.Core.Helpers;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day03
{
    internal static partial class Program
    {
        private static async Task Part01()
        {
            string text;
            using (StreamReader reader = new(InputFileHelper.GetInputFile(03)))
            {
                text = await reader.ReadToEndAsync();
            }

            Regex regex = MultiplyRegex();
            MatchCollection result = regex.Matches(text);
            int sum = result.Sum(match =>
            {
                int a = int.Parse(match.Groups["a"].Value);
                int b = int.Parse(match.Groups["b"].Value);
                return a * b;
            });
            Console.WriteLine($"Result: {sum}");
        }
        private static async Task Part02()
        {
            string text;
            using (StreamReader reader = new(InputFileHelper.GetInputFile(03)))
            {
                text = await reader.ReadToEndAsync();
            }

            Regex regex = Part02Instructions();
            MatchCollection result = regex.Matches(text);
            int sum = 0;
            bool enabled = true;
            foreach (Match match in result)
            {
                if (match.Groups["do"].Success)
                    enabled = true;
                else if (match.Groups["dont"].Success)
                    enabled = false;
                else if (match.Groups["mul"].Success && enabled)
                {
                    int a = int.Parse(match.Groups["a"].Value);
                    int b = int.Parse(match.Groups["b"].Value);
                    sum += a * b;
                }
            }

            Console.WriteLine($"Result: {sum}");
        }


        static async Task Main(string[] args)
        {
            Console.WriteLine("Part 01");
            await Part01();
            Console.WriteLine("Part 02");
            await Part02();
            Console.ReadLine();
        }

        [GeneratedRegex(@"mul\((?<a>[0-9]{1,3}),(?<b>[0-9]{1,3})\)")]
        private static partial Regex MultiplyRegex();


        [GeneratedRegex(@"(?<mul>mul\((?<a>[0-9]{1,3}),(?<b>[0-9]{1,3})\))|(?<do>do\(\))|(?<dont>don't\(\))")]
        private static partial Regex Part02Instructions();
    }
}
