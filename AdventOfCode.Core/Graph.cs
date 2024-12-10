namespace AdventOfCode.Core
{
    public class Graph<TValue> where TValue : class
    {
        private readonly List<TValue> _nodes = [];
        private readonly Dictionary<int, Dictionary<int, int>> _edges = [];
        private readonly Dictionary<int, Dictionary<int, int>> _reverseEdges = [];


        public int AddNode(TValue value)
        {
            _nodes.Add(value);
            return _nodes.Count - 1;
        }

        public TValue? GetNode(int id)
        {
            return _nodes.ElementAtOrDefault(id);
        }

        public void AddEdge(int from, int to, int cost = 1)
        {
            if (!_edges.ContainsKey(from))
                _edges.Add(from, new Dictionary<int, int>());
            _edges[from][to] = cost;

            if (!_reverseEdges.ContainsKey(to))
                _reverseEdges.Add(to, new Dictionary<int, int>());
            _reverseEdges[to][from] = cost;
        }

        public void AddBidirectionalEdge(int from, int to, int cost = 1)
        {
            AddEdge(from, to, cost);
            AddEdge(to, from, cost);
        }

        public List<(int id, int cost)> GetNeighborsOut(int id)
        {
            return _edges
                .GetValueOrDefault(id, new Dictionary<int, int>())
                .Select(kv => (kv.Key, kv.Value)).ToList();
        }

        public List<(int id, int cost)> GetNeighborsIn(int id)
        {
            return _reverseEdges
                .GetValueOrDefault(id, new Dictionary<int, int>())
                .Select(kv => (kv.Key, kv.Value)).ToList();
        }

        public List<(int id, TValue node)> GetNodesWhere(Func<TValue, bool> predicate)
        {
            List<(int, TValue)> nodes = [];
            for (int i = 0; i < _nodes.Count; ++i)
            {
                bool use = predicate.Invoke(_nodes[i]);
                if (use)
                    nodes.Add((i, _nodes[i]));
            }
            return nodes;
        }
    }
}
