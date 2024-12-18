namespace AdventOfCode.Core
{
    public class PositionalNode<TValue>
    {
        public TValue Value { get; set; }
        public int X { get; }
        public int Y { get; }


        public PositionalNode(int x, int y, TValue value)
        {
            Value = value;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{X} {Y} : {Value}";
        }

        public static explicit operator TValue(PositionalNode<TValue> value)
        {
            return value.Value;
        }

        public static bool operator ==(PositionalNode<TValue> node, TValue? value)
        {
            if (value == null || node.Value == null)
                return value == null && node.Value == null;

            return node.Value.Equals(value);
        }

        public static bool operator !=(PositionalNode<TValue> node, TValue? value)
        {
            return !(node == value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, X, Y);
        }

        protected bool Equals(PositionalNode<TValue> other)
        {
            return EqualityComparer<TValue>.Default.Equals(Value, other.Value) && X == other.X && Y == other.Y;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj switch
            {
                TValue value => value.Equals(Value),
                PositionalNode<TValue> other => Equals(other),
                _ => false
            };
        }
    }
}
