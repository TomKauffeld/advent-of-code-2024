using AdventOfCode.Core;
using AdventOfCode.Core.Helpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day14
{
    internal static partial class Program
    {
        private static async Task<(Vector2L size, List<Robot> robots)> GetInput(bool test = false)
        {
            const long testWidth = 11L;
            const long testHeight = 7L;
            const long inputWidth = 101L;
            const long inputHeight = 103L;

            List<string> lines = await InputFileHelper.GetLines(14, test);
            List<Robot> robots = [];

            foreach (string line in lines)
            {
                Match match = RobotRegex().Match(line);
                if (match.Success)
                {
                    robots.Add(new Robot(
                        new Vector2L(
                            long.Parse(match.Groups["px"].Value),
                            long.Parse(match.Groups["py"].Value)
                            ),
                        new Vector2L(
                            long.Parse(match.Groups["vx"].Value),
                            long.Parse(match.Groups["vy"].Value)
                        )
                        )
                    );
                }
            }

            return (
                new Vector2L(test ? testWidth : inputWidth, test ? testHeight : inputHeight),
                robots
            );
        }

        private static void DisplayField((Vector2L size, List<Robot> robots) field)
        {
            for (long y = 0L; y < field.size.Y; ++y)
            {
                List<Robot> lineRobots = field.robots.Where(r => r.Position.Y == y).ToList();
                for (long x = 0L; x < field.size.X; ++x)
                {
                    long count = lineRobots.LongCount(r => r.Position.X == x);
                    Console.Write(count > 0L ? $"{count}" : ".");
                }
                Console.WriteLine("");
            }
        }

        private static (List<Robot> topLeft, List<Robot> topRight, List<Robot> bottomLeft, List<Robot> bottomRight) SortQuadrants((Vector2L size, List<Robot> robots) field)
        {
            ConcurrentBag<Robot> topLeft = [];
            ConcurrentBag<Robot> topRight = [];
            ConcurrentBag<Robot> bottomLeft = [];
            ConcurrentBag<Robot> bottomRight = [];

            long midX = field.size.X / 2L;
            long midY = field.size.Y / 2L;

            Parallel.ForEach(field.robots, robot =>
            {
                if (robot.Position.X < midX)
                {
                    if (robot.Position.Y < midY)
                        topLeft.Add(robot);
                    else if (robot.Position.Y > midY)
                        bottomLeft.Add(robot);
                }
                else if (robot.Position.X > midX)
                {
                    if (robot.Position.Y < midY)
                        topRight.Add(robot);
                    else if (robot.Position.Y > midY)
                        bottomRight.Add(robot);
                }
            });

            return (
                topLeft.ToList(),
                topRight.ToList(),
                bottomLeft.ToList(),
                bottomRight.ToList()
            );
        }

        private static async Task Part01()
        {
            const long steps = 100L;
            (Vector2L size, List<Robot> robots) data = await GetInput();

            Parallel.ForEach(data.robots, robot =>
            {
                Vector2L displacement = robot.Velocity * steps;
                robot.Position = robot.StartingPosition + displacement;
                robot.Position = robot.Position.Wrap(data.size);
            });

            (List<Robot> topLeft, List<Robot> topRight, List<Robot> bottomLeft, List<Robot> bottomRight) quadrants = SortQuadrants(data);

            long topLeft = quadrants.topLeft.Count;
            long topRight = quadrants.topRight.Count;
            long bottomLeft = quadrants.bottomLeft.Count;
            long bottomRight = quadrants.bottomRight.Count;
            long total = topLeft * topRight * bottomRight * bottomLeft;
            DisplayField(data);
            //Low 218325492
            //    207790050
            //    225810288
            Console.WriteLine($"After {steps}, got {topLeft}, {topRight}, {bottomLeft}, {bottomRight}");
            Console.WriteLine($"With a total of {total}");
        }


        private static async Task WriteImage((Vector2L size, List<Robot> robots) field, string path)
        {
            using Image<Rgba32> image = new((int)field.size.X, (int)field.size.Y);

            for (long y = 0L; y < field.size.Y; ++y)
            {
                List<Robot> lineRobots = field.robots.Where(r => r.Position.Y == y).ToList();
                for (long x = 0L; x < field.size.X; ++x)
                {
                    long count = lineRobots.LongCount(r => r.Position.X == x);
                    if (count > 0L)
                        image[(int)x, (int)y] = Rgba32.ParseHex("FFF");
                    else
                        image[(int)x, (int)y] = Rgba32.ParseHex("000");
                }
            }

            await image.SaveAsPngAsync(path);
        }


        private static async Task Part02()
        {
            const long steps = 10000L;
            (Vector2L size, List<Robot> robots) data = await GetInput();

            for (long step = 0L; step <= steps; ++step)
            {
                Parallel.ForEach(data.robots, robot =>
                {
                    Vector2L displacement = robot.Velocity * step;
                    robot.Position = robot.StartingPosition + displacement;
                    robot.Position = robot.Position.Wrap(data.size);
                });
                await WriteImage(data, $"image_{step}.png");
            }
            Console.WriteLine($"Created {steps} images");
        }


        private static async Task Main(string[] args)
        {
            Console.WriteLine("Part 01");
            await Part01();
            Console.WriteLine("Part 02");
            await Part02();
            Console.ReadLine();
        }

        [GeneratedRegex("^p=(?<px>-?[0-9]+),(?<py>-?[0-9]+) v=(?<vx>-?[0-9]+),(?<vy>-?[0-9]+)$")]
        private static partial Regex RobotRegex();
    }
}
