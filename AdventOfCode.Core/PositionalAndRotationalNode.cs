namespace AdventOfCode.Core
{
    public class PositionalAndRotationalNode<TValue> : PositionalNode<TValue>
    {
        public Direction R { get; }

        public PositionalAndRotationalNode(int x, int y, Direction r, TValue value) : base(x, y, value)
        {
            R = r;
        }

        public override string ToString()
        {
            return $"{X} {Y} {R} : {Value}";
        }

        public static explicit operator TValue(PositionalAndRotationalNode<TValue> value)
        {
            return value.Value;
        }

        public static bool operator ==(PositionalAndRotationalNode<TValue> node, TValue? value)
        {
            if (value == null || node.Value == null)
                return value == null && node.Value == null;

            return node.Value.Equals(value);
        }

        public static bool operator !=(PositionalAndRotationalNode<TValue> node, TValue? value)
        {
            return !(node == value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, X, Y, R);
        }

        protected bool Equals(PositionalAndRotationalNode<TValue> other)
        {
            return EqualityComparer<TValue>.Default.Equals(Value, other.Value)
                   && X == other.X && Y == other.Y && R == other.R;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj switch
            {
                TValue value => value.Equals(Value),
                PositionalAndRotationalNode<TValue> other => Equals(other),
                _ => false
            };
        }
    }
}
