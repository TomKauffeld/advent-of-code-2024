using AdventOfCode.Core.Helpers;
using System.Collections.Concurrent;

namespace AdventOfCode.Day06
{
    internal static class Program
    {
        public const int Up = 0;
        public const int Right = 1;
        public const int Down = 2;
        public const int Left = 3;

        private static Tuple<int, int, int> GetPosition(List<string> field)
        {
            for (int y = 0; y < field.Count; ++y)
                for (int x = 0; x < field[y].Length; ++x)
                    switch (field[y][x])
                    {
                        case '^':
                            return Tuple.Create(x, y, Up);
                        case '>':
                            return Tuple.Create(x, y, Right);
                        case 'v':
                            return Tuple.Create(x, y, Down);
                        case '<':
                            return Tuple.Create(x, y, Left);
                    }

            throw new Exception("Guard not found");
        }

        private static bool IsWall(List<string> field, int x, int y)
        {
            return field[y][x] == '#';
        }

        private static bool IsInside(List<string> field, int x, int y)
        {
            return x >= 0 && y >= 0 && y < field.Count && x < field[y].Length;
        }

        private static Tuple<int, int> NextPosition(int x, int y, int r)
        {
            return r switch
            {
                Up => Tuple.Create(x, y - 1),
                Right => Tuple.Create(x + 1, y),
                Down => Tuple.Create(x, y + 1),
                Left => Tuple.Create(x - 1, y),
                _ => Tuple.Create(x, y)
            };
        }

        private static async Task Part01()
        {
            List<string> field = await InputFileHelper.GetLines(06);
            (int x, int y, int r) = GetPosition(field);
            List<(int, int)> positions = [];

            while (IsInside(field, x, y))
            {
                if (!positions.Contains((x, y)))
                    positions.Add((x, y));
                (int nextX, int nextY) = NextPosition(x, y, r);
                if (!IsInside(field, nextX, nextY))
                    break;
                if (IsWall(field, nextX, nextY))
                {
                    r = (r + 1) % 4;
                }
                else
                {
                    x = nextX;
                    y = nextY;
                }
            }
            Console.WriteLine($"Found {positions.Count} positions");
        }

        private static bool IsStuck(List<string> field)
        {
            (int x, int y, int r) = GetPosition(field);
            Dictionary<(int, int), List<int>> positions = [];
            while (IsInside(field, x, y))
            {
                if (positions.ContainsKey((x, y)) && positions[(x, y)].Contains(r))
                    return true;
                if (positions.ContainsKey((x, y)))
                    positions[(x, y)].Add(r);
                else
                    positions.Add((x, y), [r]);

                (int nextX, int nextY) = NextPosition(x, y, r);
                if (!IsInside(field, nextX, nextY))
                    break;
                if (IsWall(field, nextX, nextY))
                    r = (r + 1) % 4;
                else
                {
                    x = nextX;
                    y = nextY;
                }
            }
            return false;
        }

        private static async Task Part02()
        {
            List<string> field = await InputFileHelper.GetLines(06);
            (int x, int y, int r) = GetPosition(field);
            List<(int, int)> positions = [];
            int oldX = x;
            int oldY = y;
            while (IsInside(field, x, y))
            {
                if (!positions.Contains((x, y)))
                    positions.Add((x, y));
                (int nextX, int nextY) = NextPosition(x, y, r);
                if (!IsInside(field, nextX, nextY))
                    break;
                if (IsWall(field, nextX, nextY))
                {
                    r = (r + 1) % 4;
                }
                else
                {
                    x = nextX;
                    y = nextY;
                }
            }
            positions.Remove((oldX, oldY));
            ConcurrentBag<(int, int)> walls = [];
            Parallel.ForEach(positions, pos =>
            {
                (int posX, int posY) = pos;
                List<string> newField = field.Select((s, i) =>
                {
                    if (i != posY)
                        return s;
                    return s[..posX] + "#" + s[(posX + 1)..];
                }).ToList();

                if (IsStuck(newField))
                    walls.Add(pos);
            });
            Console.WriteLine($"Found {walls.Count} positions");
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