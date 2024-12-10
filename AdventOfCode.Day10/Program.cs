using AdventOfCode.Core;
using AdventOfCode.Core.Helpers;
using System.Collections.Concurrent;

namespace AdventOfCode.Day10
{
    internal static class Program
    {
        private static async Task<PositionalGraph<int>> ReadGraph(bool test = false)
        {
            List<string> lines = await InputFileHelper.GetLines(10, test);
            PositionalGraph<int> graph = new();

            for (int y = 0; y < lines.Count; ++y)
            {
                int valueLeft = int.MinValue;
                for (int x = 0; x < lines[y].Length; ++x)
                {
                    int value = lines[y][x] - '0';

                    graph.AddNode(x, y, value);

                    if (valueLeft == value - 1)
                        graph.AddEdge((x - 1, y), (x, y));
                    else if (valueLeft == value + 1)
                        graph.AddEdge((x, y), (x - 1, y));

                    int? valueTop = graph.GetNodeValue(x, y - 1);
                    if (valueTop == value - 1)
                        graph.AddEdge((x, y - 1), (x, y));
                    else if (valueTop == value + 1)
                        graph.AddEdge((x, y), (x, y - 1));

                    valueLeft = value;
                }
            }

            return graph;
        }


        private static async Task Part01()
        {
            PositionalGraph<int> graph = await ReadGraph();
            List<(int x, int y)> positions = graph
                .GetNodesWhere(v => v == 0)
                .Select(item => (item.node.X, item.node.Y))
                .ToList();

            ConcurrentBag<int> scores = [];

            Parallel.ForEach(positions, position =>
            {
                List<(int x, int y)> done = [];
                List<(int x, int y)> finalPositions = [];
                Queue<(int x, int y)> next = [];
                next.Enqueue(position);
                done.Add(position);

                while (next.TryDequeue(out (int x, int y) scanPosition))
                {
                    int? value = graph.GetNodeValue(scanPosition.x, scanPosition.y);
                    if (value == 9)
                    {
                        if (!finalPositions.Contains(scanPosition))
                            finalPositions.Add(scanPosition);
                    }
                    else
                    {
                        List<((int x, int y) position, int cost)> neighbors = graph.GetNeighborsOut(scanPosition.x, scanPosition.y);
                        foreach (((int x, int y) position, int cost) neighbor in neighbors)
                        {
                            if (!done.Contains(neighbor.position))
                            {
                                next.Enqueue(neighbor.position);
                                done.Add(neighbor.position);
                            }
                        }
                    }
                }
                scores.Add(finalPositions.Count);
            });

            Console.WriteLine($"Found a total score of {scores.Sum()}");
        }


        private static async Task Part02()
        {
            PositionalGraph<int> graph = await ReadGraph();
            List<(int x, int y)> positions = graph
                .GetNodesWhere(v => v == 0)
                .Select(item => (item.node.X, item.node.Y))
                .ToList();

            ConcurrentBag<int> scores = [];

            Parallel.ForEach(positions, position =>
            {
                List<(int x, int y)> finalPositions = [];
                Queue<(int x, int y)> next = [];
                next.Enqueue(position);

                while (next.TryDequeue(out (int x, int y) scanPosition))
                {
                    int? value = graph.GetNodeValue(scanPosition.x, scanPosition.y);
                    if (value == 9)
                    {
                        finalPositions.Add(scanPosition);
                    }
                    else
                    {
                        List<((int x, int y) position, int cost)> neighbors = graph.GetNeighborsOut(scanPosition.x, scanPosition.y);
                        foreach (((int x, int y) position, int cost) neighbor in neighbors)
                        {
                            next.Enqueue(neighbor.position);
                        }
                    }
                }
                scores.Add(finalPositions.Count);
            });

            Console.WriteLine($"Found a total score of {scores.Sum()}");
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
