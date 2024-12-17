using AdventOfCode.Core;
using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day16
{
    internal static class Program
    {
        private static async Task<(PositionalAndRotationalGraph<char> graph, Vector2L start, Vector2L end)> GetInput(bool test = false)
        {
            const int costTurn = 1000;
            const int costAdvance = 1;

            List<string> lines = await InputFileHelper.GetLines(16, test);
            PositionalAndRotationalGraph<char> graph = new();
            Vector2L start = new(0, 0);
            Vector2L end = new(0, 0);

            for (int y = 0; y < lines.Count; ++y)
            {
                for (int x = 0; x < lines[y].Length; ++x)
                {
                    if (lines[y][x] == '#')
                        continue;
                    if (lines[y][x] == 'S')
                        start = new Vector2L(x, y);
                    if (lines[y][x] == 'E')
                        end = new Vector2L(x, y);

                    int up = graph.AddNode(x, y, Direction.Up, lines[y][x]);
                    int right = graph.AddNode(x, y, Direction.Right, lines[y][x]);
                    int down = graph.AddNode(x, y, Direction.Down, lines[y][x]);
                    int left = graph.AddNode(x, y, Direction.Left, lines[y][x]);

                    graph.AddBidirectionalEdge(up, right, costTurn);
                    graph.AddBidirectionalEdge(right, down, costTurn);
                    graph.AddBidirectionalEdge(down, left, costTurn);
                    graph.AddBidirectionalEdge(left, up, costTurn);

                    if (y > 0 && lines[y - 1][x] != '#')
                    {
                        graph.AddEdge(
                            (x, y, Direction.Up),
                            (x, y - 1, Direction.Up),
                            costAdvance
                        );
                        graph.AddEdge(
                            (x, y - 1, Direction.Down),
                            (x, y, Direction.Down),
                            costAdvance
                        );
                    }

                    if (x > 0 && lines[y][x - 1] != '#')
                    {
                        graph.AddEdge(
                            (x, y, Direction.Left),
                            (x - 1, y, Direction.Left),
                            costAdvance
                        );
                        graph.AddEdge(
                            (x - 1, y, Direction.Right),
                            (x, y, Direction.Right),
                            costAdvance
                        );
                    }
                }
            }

            return (graph, start, end);
        }

        private static async Task Part01()
        {
            const Direction startDirection = Direction.Right;
            (PositionalAndRotationalGraph<char> graph, Vector2L start, Vector2L end) = await GetInput();

            int from = graph.GetNodeId((int)start.X, (int)start.Y, startDirection)
                       ?? throw new Exception("Start node not found");
            int to = graph.GetNodeId((int)end.X, (int)end.Y, Direction.Up)
                     ?? throw new Exception("End node not found");

            (int cost, List<int> reversePath)? result = graph.FindPath(from, to);

            if (!result.HasValue)
                throw new Exception("No path found");

            int currentIndex = 0;
            bool search = true;
            int cost = result.Value.cost;
            while (search)
            {
                (int x, int y, Direction r)? pos = graph.GetPosition(result.Value.reversePath[currentIndex + 1]);
                int? edge = graph.GetEdge(currentIndex + 1, currentIndex);
                if (pos.HasValue && edge.HasValue && pos.Value.x == end.X && pos.Value.y == end.Y)
                {
                    ++currentIndex;
                    cost -= edge.Value;
                }
                else
                {
                    search = false;
                }
            }

            Console.WriteLine($"Found cost: {cost}");

        }


        private static async Task Part02()
        {
            const Direction startDirection = Direction.Right;
            (PositionalAndRotationalGraph<char> graph, Vector2L start, Vector2L end) = await GetInput();

            int from = graph.GetNodeId((int)start.X, (int)start.Y, startDirection)
                       ?? throw new Exception("Start node not found");
            int to = graph.GetNodeId((int)end.X, (int)end.Y, Direction.Up)
                     ?? throw new Exception("End node not found");

            (int cost, List<List<int>> reversePaths)? result = graph.FindPaths(from, to);

            if (!result.HasValue)
                throw new Exception("No path found");


            Console.WriteLine($"Found {result.Value.reversePaths.Count} Paths");

            List<(int x, int y)> positions = [];
            foreach (List<int> path in result.Value.reversePaths)
            {
                foreach (int id in path)
                {
                    (int x, int y, Direction r)? position = graph.GetPosition(id);
                    if (position.HasValue && !positions.Contains((position.Value.x, position.Value.y)))
                    {
                        positions.Add((position.Value.x, position.Value.y));
                    }
                }
            }
            Console.WriteLine($"Found {positions.Count} positions");
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
