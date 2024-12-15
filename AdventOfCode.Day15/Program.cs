using AdventOfCode.Core;
using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day15
{
    internal static class Program
    {
        private static async Task<(Field field, List<Direction> directions, (int x, int y) position)> GetInput(bool test = false)
        {
            List<string> lines = await InputFileHelper.GetLines(15, test);
            List<Field.FieldItem> items = [];
            List<Direction> directions = [];
            int width = 0;
            int height = 0;
            (int x, int y) position = (0, 0);

            for (int y = 0; y < lines.Count; ++y)
            {
                if (lines[y].StartsWith('#'))
                {
                    width = Math.Max(width, lines[y].Length);
                    height = y + 1;
                    for (int x = 0; x < lines[y].Length; ++x)
                    {
                        switch (lines[y][x])
                        {
                            case '#':
                                items.Add(Field.FieldItem.Wall);
                                break;
                            case '.':
                                items.Add(Field.FieldItem.Nothing);
                                break;
                            case 'O':
                                items.Add(Field.FieldItem.Box);
                                break;
                            case '@':
                                items.Add(Field.FieldItem.Nothing);
                                position = (x, y);
                                break;

                        }
                    }
                }
                else
                {
                    for (int x = 0; x < lines[y].Length; ++x)
                    {
                        switch (lines[y][x])
                        {
                            case '^':
                                directions.Add(Direction.Up);
                                break;
                            case '>':
                                directions.Add(Direction.Right);
                                break;
                            case 'v':
                                directions.Add(Direction.Down);
                                break;
                            case '<':
                                directions.Add(Direction.Left);
                                break;

                        }
                    }
                }
            }

            return (
                new Field(width, height, items.ToArray()),
                directions,
                position
            );
        }

        private static async Task Part01()
        {
            (Field field, List<Direction> directions, (int x, int y) position) = await GetInput();

            foreach (Direction direction in directions)
            {
                (int x, int y) next = Field.GetNextLocation(position.x, position.y, direction);
                if (field.Move(next.x, next.y, direction))
                    position = next;

                //Console.WriteLine(field.ToString((position.x, position.y, '@')));
            }

            long sum = 0L;
            for (int y = 0; y < field.Height; ++y)
            {
                for (int x = 0; x < field.Width; ++x)
                {
                    if (field.GetItem(x, y) == Field.FieldItem.Box)
                    {
                        long gps = y * 100L + x;
                        sum += gps;
                    }
                }
            }

            Console.WriteLine($"Found GPS: {sum}");
        }


        private static async Task Part02()
        {

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
