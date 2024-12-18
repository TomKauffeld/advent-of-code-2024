using System.Text.RegularExpressions;

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

        public static async Task<List<List<int>>> GetSeparatedNumbers(int day, bool test = false, char separator = ' ')
        {
            List<List<int>> result = [];

            await using FileStream fileStream = GetInputFile(day, test);
            using StreamReader reader = new(fileStream);

            while (await reader.ReadLineAsync() is { } line)
            {
                line = line.Trim();
                if (line.Length < 1)
                    continue;
                string[] parts = line.Split(separator);
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

        public static async Task<List<string>> GetLines(int day, bool test = false)
        {
            List<string> lines = [];

            await using FileStream fileStream = GetInputFile(day, test);
            using StreamReader reader = new(fileStream);

            while (await reader.ReadLineAsync() is { } line)
            {
                line = line.Trim();
                if (line.Length < 1)
                    continue;
                lines.Add(line);
            }

            return lines;
        }

        public static async Task<Tuple<List<List<int>>, List<List<int>>>> GetPipeToCommaSeparatedNumbers(int day, bool test = false)
        {
            List<List<int>> firstList = [];
            List<List<int>> secondList = [];

            await using (FileStream fileStream = GetInputFile(day, test))
            {
                using (StreamReader reader = new(fileStream))
                {
                    while (await reader.ReadLineAsync() is { } line)
                    {
                        line = line.Trim();
                        if (line.Length < 1)
                            break;
                        string[] parts = line.Split('|');
                        firstList.Add(parts.Select(int.Parse).ToList());
                    }

                    while (await reader.ReadLineAsync() is { } line)
                    {
                        line = line.Trim();
                        if (line.Length < 1)
                            break;
                        string[] parts = line.Split(',');
                        secondList.Add(parts.Select(int.Parse).ToList());
                    }
                }
            }

            return Tuple.Create(firstList, secondList);
        }


        public static async Task<List<Tuple<long, List<long>>>> GetNumberToNumbersList(int day, bool test = false)
        {
            List<Tuple<long, List<long>>> lists = [];
            await using FileStream fileStream = GetInputFile(day, test);
            using StreamReader reader = new(fileStream);

            while (await reader.ReadLineAsync() is { } line)
            {
                try
                {
                    line = line.Trim();
                    if (line.Length < 1)
                        continue;
                    string[] mainParts = line.Split(':');
                    if (mainParts.Length != 2)
                        throw new Exception($"Invalid line {line}");
                    long target = long.Parse(mainParts[0]);
                    List<long> numbers = mainParts[1]
                        .Split(' ')
                        .Select(s => s.Trim())
                        .Where(s => s.Length > 0)
                        .Select(long.Parse)
                        .ToList();

                    lists.Add(Tuple.Create(target, numbers));
                }
                catch (Exception e)
                {
                    throw new Exception($"Error with line {line}", e);
                }
            }

            return lists;
        }


        public static async Task<Field<TCustomData>> GetField<TCustomData>(int day, bool test = false) where TCustomData : struct
        {
            int width = 0;
            int height = 0;
            List<char> data = [];
            await using FileStream fileStream = GetInputFile(day, test);
            using StreamReader reader = new(fileStream);
            while (await reader.ReadLineAsync() is { } line)
            {
                try
                {
                    line = line.Trim();
                    if (line.Length < 1)
                        continue;
                    if (width == 0)
                        width = line.Length;
                    else if (width != line.Length)
                        throw new Exception($"Expected width of {width} but found {line.Length}");
                    data.AddRange(line);
                    ++height;
                }
                catch (Exception e)
                {
                    throw new Exception($"Error with line {line}", e);
                }
            }

            return new Field<TCustomData>(data.ToArray(), width, height);
        }


        [GeneratedRegex("^(?<a>[0-9]+) +(?<b>[0-9]+)$")]
        private static partial Regex DualNumberRegex();
    }
}
