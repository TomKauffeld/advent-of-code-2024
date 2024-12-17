using System.Collections.Concurrent;

namespace AdventOfCode.Core
{
    public class Graph<TValue> where TValue : class
    {
        private readonly object _nodesLock = new();
        private readonly List<TValue> _nodes = [];
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, int>> _edges = [];
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, int>> _reverseEdges = [];

        public int CountNodes()
        {
            lock (_nodesLock)
            {
                return _nodes.Count;
            }
        }

        public int AddNode(TValue value)
        {
            lock (_nodesLock)
            {
                _nodes.Add(value);
                return _nodes.Count - 1;
            }
        }

        public TValue? GetNode(int id)
        {
            lock (_nodesLock)
            {
                return _nodes.ElementAtOrDefault(id);
            }
        }
        public int? GetEdge(int from, int to)
        {
            if (_edges.TryGetValue(from, out ConcurrentDictionary<int, int>? edges) && edges.TryGetValue(to, out int value))
                return value;

            return null;
        }

        public void AddEdge(int from, int to, int cost = 1)
        {
            _edges.AddOrUpdate(from,
                _ => new ConcurrentDictionary<int, int>([new KeyValuePair<int, int>(to, cost)]),
                (_, value) =>
            {
                value.AddOrUpdate(to, cost, (_, _) => cost);
                return value;
            });
            _reverseEdges.AddOrUpdate(to,
                _ => new ConcurrentDictionary<int, int>([new KeyValuePair<int, int>(from, cost)]),
                (_, value) =>
                {
                    value.AddOrUpdate(from, cost, (_, _) => cost);
                    return value;
                });
        }

        public void AddBidirectionalEdge(int from, int to, int cost = 1)
        {
            AddEdge(from, to, cost);
            AddEdge(to, from, cost);
        }

        public List<(int id, int cost)> GetNeighborsOut(int id)
        {
            return _edges.TryGetValue(id, out ConcurrentDictionary<int, int>? list)
                ? list.Select(kv => (kv.Key, kv.Value)).ToList()
                : [];
        }

        public List<(int id, int cost)> GetNeighborsIn(int id)
        {
            return _reverseEdges.TryGetValue(id, out ConcurrentDictionary<int, int>? list)
                ? list.Select(kv => (kv.Key, kv.Value)).ToList()
                : [];
        }

        public List<(int id, TValue node)> GetNodesWhere(Func<TValue, bool> predicate)
        {
            List<(int, TValue)> nodes = [];

            lock (_nodesLock)
            {
                for (int i = 0; i < _nodes.Count; ++i)
                {
                    bool use = predicate.Invoke(_nodes[i]);
                    if (use)
                        nodes.Add((i, _nodes[i]));
                }
            }

            return nodes;
        }

    }
}
