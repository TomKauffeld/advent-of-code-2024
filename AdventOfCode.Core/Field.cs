namespace AdventOfCode.Core
{
    public class Field<TCustomData> where TCustomData : struct
    {
        private readonly char[] _field;
        public int Width { get; }
        public int Height { get; }
        private readonly TCustomData?[] _data;

        public Field(char[] field, int width, int height)
        {
            if (width * height != field.Length)
                throw new ArgumentException($"Expected size {Width * Height} but found {field.Length}", nameof(field));
            _field = field;
            Width = width;
            Height = height;
            _data = new TCustomData?[width * height];
        }

        public char Get(int x, int y)
        {
            if (x < 0 || x >= Width)
                return '\0';
            if (y < 0 || y >= Height)
                return '\0';
            return _field[x + y * Width];
        }

        public TCustomData? GetCustomData(int x, int y)
        {
            if (x < 0 || x >= Width)
                return null;
            if (y < 0 || y >= Height)
                return null;
            return _data[x + y * Width];
        }

        public void SetCustomData(int x, int y, TCustomData? value)
        {
            if (x < 0 || x >= Width)
                return;
            if (y < 0 || y >= Height)
                return;
            _data[x + y * Width] = value;
        }
    }
}
