﻿using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Helpers
{
    public static partial class InputFileHelper
    {
        public static string GetInputFilePath(int day, bool test = false)
        {
            return Path.Combine(
                Environment.CurrentDirectory,
                "..",
                "..",
                "..",
                "..",
                "AdventOfCode.Data",
                $"day{day:00}",
                test ? "test.txt" : "input.txt"
            );
        }

        public static FileStream GetInputFile(int day, bool test = false)
        {
            string path = GetInputFilePath(day, test);
            return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public static async Task<List<List<int>>> GetSpaceSeparatedNumbers(int day, bool test = false)
        {
            List<List<int>> result = [];

            await using FileStream fileStream = GetInputFile(day, test);
            using StreamReader reader = new(fileStream);

            while (await reader.ReadLineAsync() is { } line)
            {
                line = line.Trim();
                if (line.Length < 1)
                    continue;
                string[] parts = line.Split(' ');
                List<int> items = parts.Select(int.Parse).ToList();
                result.Add(items);
            }

            return result;
        }

        public static async Task<Tuple<List<int>, List<int>>> GetDualNumbers(int day, bool test = false)
        {
            List<int> firstList = [];
            List<int> secondList = [];

            await using (FileStream fileStream = GetInputFile(day, test))
            {
                using (StreamReader reader = new(fileStream))
                {
                    Regex regex = DualNumberRegex();
                    while (await reader.ReadLineAsync() is { } line)
                    {
                        line = line.Trim();
                        if (line.Length < 1)
                            continue;
                        Match match = regex.Match(line);
                        if (match.Success &&
                            int.TryParse(match.Groups["a"].Value, out int a) &&
                            int.TryParse(match.Groups["b"].Value, out int b))
                        {
                            firstList.Add(a);
                            secondList.Add(b);
                        }
                        else
                        {
                            throw new Exception($"Invalid line found: {line}");
                        }

                    }
                }
            }

            return Tuple.Create(firstList, secondList);
        }

        [GeneratedRegex("^(?<a>[0-9]+) +(?<b>[0-9]+)$")]
        private static partial Regex DualNumberRegex();
    }
}
