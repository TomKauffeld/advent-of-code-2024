using AdventOfCode.Core;
using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day18
{
    internal static class Program
    {
        private static async Task<(PositionalGraph<char> graph, Vector2L size, List<(int x, int y)> positions)> GetInput(bool test = false)
        {
            List<List<int>> numbers = await InputFileHelper.GetSeparatedNumbers(18, test, ',');
            Vector2L size = test ? new Vector2L(6L, 6L) : new Vector2L(70L, 70L);

            List<(int x, int y)> positions = numbers
                .Select(list => (list[0], list[1]))
                .ToList();

            PositionalGraph<char> graph = new();

            for (int y = 0; y <= size.Y; ++y)
            {
                for (int x = 0; x <= size.X; ++x)
                {
                    if (graph.GetNodeId(x, y) == null)
                    {
                        graph.AddNode(x, y, '.');

                        if (x > 0)
                            graph.AddBidirectionalEdge((x, y), (x - 1, y));
                        if (y > 0)
                            graph.AddBidirectionalEdge((x, y), (x, y - 1));
                    }
                }
            }

            return (graph, size, positions);
        }

        private static void RemovePositions(PositionalGraph<char> graph, List<(int x, int y)> positions, int from = 0, int to = int.MaxValue)
        {
            for (int i = from; i < to && i < positions.Count; ++i)
            {
                int x = positions[i].x;
                int y = positions[i].y;
                graph.RemoveBidirectionalEdge((x, y), (x - 1, y));
                graph.RemoveBidirectionalEdge((x, y), (x, y - 1));
                graph.RemoveBidirectionalEdge((x, y), (x + 1, y));
                graph.RemoveBidirectionalEdge((x, y), (x, y + 1));
                int? id = graph.GetNodeId(x, y);

                if (id.HasValue)
                {
                    PositionalNode<char>? node = graph.GetNode(id.Value);
                    if (node != null)
                    {
                        node.Value = '#';
                    }
                }
            }
        }


        private static async Task Part01()
        {
            (PositionalGraph<char> graph, Vector2L size, List<(int x, int y)> positions) = await GetInput();

            RemovePositions(graph, positions, 0, 1024);

            int start = graph.GetNodeId(0, 0) ?? throw new Exception("Start not found");
            int end = graph.GetNodeId((int)size.X, (int)size.Y) ?? throw new Exception("End not found");

            (int cost, List<int> reversePath)? result = graph.FindPath(start, end);

            Console.WriteLine($"Found path {result?.cost}");
        }

        private static int FindFirstBlocking(PositionalGraph<char> graph, List<(int x, int y)> positions, List<int> reversePath, int offset = 0)
        {
            for (int i = offset; i < positions.Count; ++i)
            {
                int? nodeId = graph.GetNodeId(positions[i].x, positions[i].y);
                if (nodeId.HasValue && reversePath.Contains(nodeId.Value))
                    return i;
            }

            return -1;
        }

        private static async Task Part02()
        {
            (PositionalGraph<char> graph, Vector2L size, List<(int x, int y)> positions) = await GetInput();
            RemovePositions(graph, positions, 0, 1024);
            int start = graph.GetNodeId(0, 0) ?? throw new Exception("Start not found");
            int end = graph.GetNodeId((int)size.X, (int)size.Y) ?? throw new Exception("End not found");
            int offset = 1024;

            (int cost, List<int> reversePath)? result = graph.FindPath(start, end);

            while (result.HasValue)
            {
                int index = FindFirstBlocking(graph, positions, result.Value.reversePath, offset);
                if (index >= 0)
                {
                    RemovePositions(graph, positions, offset, index + 1);

                    offset = index + 1;
                }
                else
                {
                    break;
                }
                result = graph.FindPath(start, end);
            }

            (int x, int y) = positions[offset - 1];
            Console.WriteLine($"Using {offset - 1} bytes: {x},{y}");
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
