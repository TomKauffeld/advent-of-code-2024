namespace AdventOfCode.Core
{
    public class PositionalGraph<TValue> : Graph<PositionalNode<TValue>> where TValue : struct
    {
        private readonly Dictionary<(int, int), int> _positions = [];

        public int AddNode(int x, int y, TValue value)
        {
            if (_positions.ContainsKey((x, y)))
                throw new Exception($"Position {x} {y} already set");
            int id = AddNode(new PositionalNode<TValue>(x, y, value));
            _positions[(x, y)] = id;
            return id;
        }


        public void AddEdge((int x, int y) from, (int x, int y) to, int cost = 1)
        {
            int? fromId = GetNodeId(from.x, from.y);
            int? toId = GetNodeId(to.x, to.y);
            if (fromId.HasValue && toId.HasValue)
                AddEdge(fromId.Value, toId.Value, cost);
        }

        public void AddBidirectionalEdge((int x, int y) from, (int x, int y) to, int cost = 1)
        {
            AddEdge(from, to, cost);
            AddEdge(to, from, cost);
        }

        public int? GetNodeId(int x, int y)
        {
            return _positions.TryGetValue((x, y), out int id) ? id : null;
        }

        public TValue? GetNodeValue(int x, int y)
        {
            int? id = GetNodeId(x, y);
            return id.HasValue ? GetNode(id.Value)?.Value : null;
        }

        public (int x, int y)? GetPosition(int nodeId)
        {
            PositionalNode<TValue>? node = GetNode(nodeId);
            return node != null ? (node.X, node.Y) : null;
        }

        public List<((int x, int y) position, int cost)> GetNeighborsOut(int x, int y)
        {
            int? id = GetNodeId(x, y);
            if (!id.HasValue)
                return [];

            return GetNeighborsOut(id.Value).Select(item =>
            {
                (int x, int y)? position = GetPosition(item.id);
                if (!position.HasValue)
                    throw new Exception();
                return (position.Value, item.cost);
            }).ToList();
        }

        public List<((int x, int y) position, int cost)> GetNeighborsIn(int x, int y)
        {
            int? id = GetNodeId(x, y);
            if (!id.HasValue)
                return [];

            return GetNeighborsIn(id.Value).Select(item =>
            {
                (int x, int y)? position = GetPosition(item.id);
                if (!position.HasValue)
                    throw new Exception();
                return (position.Value, item.cost);
            }).ToList();
        }

        public List<(int id, PositionalNode<TValue> node)> GetNodesWhere(Func<TValue, bool> predicate)
        {
            return GetNodesWhere(node => predicate.Invoke(node.Value))
                .ToList();
        }
    }
}
