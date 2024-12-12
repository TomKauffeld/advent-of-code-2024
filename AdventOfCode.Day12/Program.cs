using AdventOfCode.Core;
using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day12
{
    internal static class Program
    {

        private static bool IsSame(Field<int> field, int x, int y, char data)
        {
            return x >= 0 && y >= 0 &&
                   x < field.Width && y < field.Height &&
                   field.Get(x, y) == data;
        }

        private static bool IsSameAndNoData(Field<int> field, int x, int y, char data)
        {
            return IsSame(field, x, y, data) &&
                   field.GetCustomData(x, y) == null;
        }

        private static int CalculateRegions(Field<int> field)
        {
            int nextId = 0;
            for (int y = 0; y < field.Height; ++y)
            {
                for (int x = 0; x < field.Width; ++x)
                {
                    int? value = field.GetCustomData(x, y);
                    if (value != null)
                        continue;
                    char data = field.Get(x, y);
                    Stack<(int x, int y)> explore = new();
                    explore.Push((x, y));
                    while (explore.TryPop(out (int x, int y) position))
                    {
                        if (field.GetCustomData(position.x, position.y) != null)
                            continue;
                        field.SetCustomData(position.x, position.y, nextId);

                        if (IsSameAndNoData(field, position.x - 1, position.y, data))
                            explore.Push((position.x - 1, position.y));
                        if (IsSameAndNoData(field, position.x, position.y - 1, data))
                            explore.Push((position.x, position.y - 1));
                        if (IsSameAndNoData(field, position.x + 1, position.y, data))
                            explore.Push((position.x + 1, position.y));
                        if (IsSameAndNoData(field, position.x, position.y + 1, data))
                            explore.Push((position.x, position.y + 1));
                    }

                    ++nextId;
                }
            }

            return nextId;
        }

        private static (int perimeter, int area)[] CalculateRegionSizes(Field<int> field, int nbRegions)
        {
            (int perimeter, int area)[] sizes = new (int perimeter, int area)[nbRegions];

            for (int y = 0; y < field.Height; ++y)
            {
                for (int x = 0; x < field.Width; ++x)
                {
                    int? region = field.GetCustomData(x, y);
                    if (region == null || region < 0 || region >= nbRegions)
                        throw new Exception($"Invalid Region: {x}, {y} : {region}");

                    char data = field.Get(x, y);

                    ++sizes[region.Value].area;

                    if (!IsSame(field, x - 1, y, data))
                        ++sizes[region.Value].perimeter;
                    if (!IsSame(field, x, y - 1, data))
                        ++sizes[region.Value].perimeter;
                    if (!IsSame(field, x + 1, y, data))
                        ++sizes[region.Value].perimeter;
                    if (!IsSame(field, x, y + 1, data))
                        ++sizes[region.Value].perimeter;
                }
            }
            return sizes;
        }


        private static async Task Part01()
        {
            Field<int> field = await InputFileHelper.GetField<int>(12);
            int nbRegions = CalculateRegions(field);
            Console.WriteLine($"Found {nbRegions} regions");
            (int perimeter, int area)[] sizes = CalculateRegionSizes(field, nbRegions);
            Console.WriteLine($"Calculated {nbRegions} sizes");

            int[] prices = sizes.Select(i => i.perimeter * i.area).ToArray();
            Console.WriteLine($"Calculated {nbRegions} prices");
            int price = prices.Sum();
            Console.WriteLine($"Total price {price}");
        }

        private static (int sides, int area)[] CalculateRegionSizeAndSides(Field<int> field, int nbRegions)
        {
            (int sides, int area)[] sizes = new (int perimeter, int area)[nbRegions];

            for (int y = 0; y < field.Height; ++y)
            {
                for (int x = 0; x < field.Width; ++x)
                {
                    int? region = field.GetCustomData(x, y);
                    if (region == null || region < 0 || region >= nbRegions)
                        throw new Exception($"Invalid Region: {x}, {y} : {region}");

                    char data = field.Get(x, y);

                    ++sizes[region.Value].area;

                    bool sameTop = IsSame(field, x, y - 1, data);
                    bool sameLeft = IsSame(field, x - 1, y, data);
                    bool sameRight = IsSame(field, x + 1, y, data);
                    bool sameBottom = IsSame(field, x, y + 1, data);
                    bool sameTopLeft = IsSame(field, x - 1, y - 1, data);
                    bool sameBottomLeft = IsSame(field, x - 1, y + 1, data);
                    bool sameTopRight = IsSame(field, x + 1, y - 1, data);

                    if (!sameTop && !(sameLeft && !sameTopLeft))
                        ++sizes[region.Value].sides;
                    if (!sameLeft && !(sameTop && !sameTopLeft))
                        ++sizes[region.Value].sides;
                    if (!sameRight && !(sameTop && !sameTopRight))
                        ++sizes[region.Value].sides;
                    if (!sameBottom && !(sameLeft && !sameBottomLeft))
                        ++sizes[region.Value].sides;
                }
            }
            return sizes;
        }

        private static async Task Part02()
        {
            Field<int> field = await InputFileHelper.GetField<int>(12);
            int nbRegions = CalculateRegions(field);
            Console.WriteLine($"Found {nbRegions} regions");
            (int sides, int area)[] sizes = CalculateRegionSizeAndSides(field, nbRegions);
            Console.WriteLine($"Calculated {nbRegions} sizes");

            int[] prices = sizes.Select(i => i.sides * i.area).ToArray();
            Console.WriteLine($"Calculated {nbRegions} prices");
            int price = prices.Sum();
            Console.WriteLine($"Total price {price}");
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
