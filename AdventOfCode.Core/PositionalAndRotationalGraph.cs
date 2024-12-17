using System.Collections.Concurrent;

namespace AdventOfCode.Core
{
    public class PositionalAndRotationalGraph<TValue> : Graph<PositionalAndRotationalNode<TValue>> where TValue : struct
    {
        private readonly ConcurrentDictionary<(int x, int y, Direction r), int> _positions = [];

        public int AddNode(int x, int y, Direction r, TValue value)
        {
            if (_positions.ContainsKey((x, y, r)))
                throw new Exception($"Position {x} {y} {r} already set");
            int id = AddNode(new PositionalAndRotationalNode<TValue>(x, y, r, value));
            _positions[(x, y, r)] = id;
            return id;
        }


        public void AddEdge((int x, int y, Direction r) from, (int x, int y, Direction r) to, int cost = 1)
        {
            int? fromId = GetNodeId(from.x, from.y, from.r);
            int? toId = GetNodeId(to.x, to.y, to.r);
            if (fromId.HasValue && toId.HasValue)
                AddEdge(fromId.Value, toId.Value, cost);
        }

        public void AddBidirectionalEdge((int x, int y, Direction r) from, (int x, int y, Direction r) to, int cost = 1)
        {
            AddEdge(from, to, cost);
            AddEdge(to, from, cost);
        }

        public int? GetNodeId(int x, int y, Direction r)
        {
            // ReSharper disable once InconsistentlySynchronizedField
            return _positions.TryGetValue((x, y, r), out int id) ? id : null;
        }

        public TValue? GetNodeValue(int x, int y, Direction r)
        {
            int? id = GetNodeId(x, y, r);
            return id.HasValue ? GetNode(id.Value)?.Value : null;
        }

        public (int x, int y, Direction r)? GetPosition(int nodeId)
        {
            PositionalAndRotationalNode<TValue>? node = GetNode(nodeId);
            return node != null ? (node.X, node.Y, node.R) : null;
        }

        public List<((int x, int y, Direction r) position, int cost)> GetNeighborsOut(int x, int y, Direction r)
        {
            int? id = GetNodeId(x, y, r);
            if (!id.HasValue)
                return [];

            return GetNeighborsOut(id.Value).Select(item =>
            {
                (int x, int y, Direction r)? position = GetPosition(item.id);
                if (!position.HasValue)
                    throw new Exception();
                return (position.Value, item.cost);
            }).ToList();
        }

        public List<((int x, int y, Direction r) position, int cost)> GetNeighborsIn(int x, int y, Direction r)
        {
            int? id = GetNodeId(x, y, r);
            if (!id.HasValue)
                return [];

            return GetNeighborsIn(id.Value).Select(item =>
            {
                (int x, int y, Direction r)? position = GetPosition(item.id);
                if (!position.HasValue)
                    throw new Exception();
                return (position.Value, item.cost);
            }).ToList();
        }

        public List<(int id, PositionalAndRotationalNode<TValue> node)> GetNodesWhere(Func<TValue, bool> predicate)
        {
            return GetNodesWhere(node => predicate.Invoke(node.Value))
                .ToList();
        }

    }
}
