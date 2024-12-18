namespace AdventOfCode.Core.Extensions
{
    public static class GraphExtensions
    {

        private static bool TryGetFirst(this Dictionary<int, int> list, out int key, out int value)
        {
            key = list.Keys.FirstOrDefault(-1);
            if (key < 0)
            {
                value = -1;
                return false;
            }

            value = list[key];
            foreach (KeyValuePair<int, int> item in list)
            {
                if (item.Value < value)
                {
                    value = item.Value;
                    key = item.Key;
                }
            }

            if (value == int.MaxValue)
                return false;

            list.Remove(key);

            return true;
        }

        public static (int cost, List<int> reversePath)? FindPath<TValue>(this Graph<TValue> graph, int from, int to) where TValue : class
        {

            int totalNodes = graph.CountNodes();
            Dictionary<int, int> unvisited = new(totalNodes);
            int[] paths = new int[totalNodes];

            for (int i = 0; i < totalNodes; ++i)
            {
                unvisited.Add(i, int.MaxValue);
                paths[i] = -1;
            }

            unvisited[from] = 0;

            while (unvisited.TryGetFirst(out int next, out int cost))
            {
                if (next == to)
                {
                    List<int> reversePath = [];
                    while (next != from)
                    {
                        reversePath.Add(next);
                        next = paths[next];
                    }

                    return (cost, reversePath);
                }

                List<(int id, int cost)> neighbors = graph
                    .GetNeighborsOut(next)
                    .Where(node => unvisited.ContainsKey(node.id))
                    .ToList();

                foreach ((int id, int cost) neighbor in neighbors)
                {
                    int currentCost = unvisited[neighbor.id];
                    int newCost = cost + neighbor.cost;

                    if (newCost < currentCost)
                    {
                        unvisited[neighbor.id] = newCost;
                        paths[neighbor.id] = next;
                    }
                }
            }

            return null;
        }


        private static List<List<int>> BuildReversePaths(List<int>[] paths, int from, int to)
        {
            List<List<int>> results = [];

            if (from == to)
                return [[]];

            foreach (int id in paths[from])
            {
                foreach (List<int> path in BuildReversePaths(paths, id, to))
                {
                    path.Add(from);
                    results.Add(path);
                }
            }
            return results;
        }

        public static (int cost, List<List<int>> reversePath)? FindPaths<TValue>(this Graph<TValue> graph, int from, int to) where TValue : class
        {

            int totalNodes = graph.CountNodes();
            Dictionary<int, int> unvisited = new(totalNodes);
            List<int>[] paths = new List<int>[totalNodes];

            for (int i = 0; i < totalNodes; ++i)
            {
                unvisited.Add(i, int.MaxValue);
                paths[i] = [];
            }

            unvisited[from] = 0;

            while (unvisited.TryGetFirst(out int next, out int cost))
            {
                if (next == to)
                {
                    List<List<int>> reversePath = BuildReversePaths(paths, to, from);

                    return (cost, reversePath);
                }

                List<(int id, int cost)> neighbors = graph
                    .GetNeighborsOut(next)
                    .Where(node => unvisited.ContainsKey(node.id))
                    .ToList();

                foreach ((int id, int cost) neighbor in neighbors)
                {
                    int currentCost = unvisited[neighbor.id];
                    int newCost = cost + neighbor.cost;

                    if (newCost < currentCost)
                    {
                        unvisited[neighbor.id] = newCost;
                        paths[neighbor.id] = [next];
                    }
                    else if (newCost == currentCost && !paths[neighbor.id].Contains(next))
                    {
                        paths[neighbor.id].Add(next);
                    }
                }
            }

            return null;
        }

    }
}
