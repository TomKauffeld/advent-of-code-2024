namespace AdventOfCode.Core
{
    public class Vector2L
    {
        public long X { get; set; }
        public long Y { get; set; }

        public Vector2L(long x, long y)
        {
            X = x;
            Y = y;
        }


        public static Vector2L operator +(Vector2L a, Vector2L b)
        {
            return new Vector2L(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2L operator *(Vector2L v, long m)
        {
            return new Vector2L(v.X * m, v.Y * m);
        }

        public static Vector2L operator *(long m, Vector2L v) => v * m;

        public Vector2L Wrap(Vector2L size)
        {
            long x = X % size.X;
            long y = Y % size.Y;

            if (x < 0L)
                x = size.X + x;
            if (y < 0L)
                y = size.Y + y;

            return new Vector2L(x, y);
        }
    }
}
