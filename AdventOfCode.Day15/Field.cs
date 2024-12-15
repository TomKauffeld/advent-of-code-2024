using AdventOfCode.Core;
using System.Text;

namespace AdventOfCode.Day15
{
    internal class Field
    {
        public enum FieldItem
        {
            Nothing,
            Wall,
            Box,
            LeftBox,
            RightBox,
        }

        public int Width { get; }
        public int Height { get; }

        private readonly FieldItem[] _field;

        public Field(int width, int height, FieldItem[] field)
        {
            if (width * height != field.Length)
                throw new ArgumentException("Invalid field size");
            _field = field;
            Width = width;
            Height = height;
        }

        private int GetIndex(int x, int y) => x + y * Width;

        public FieldItem GetItem(int x, int y)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y));
            return _field[GetIndex(x, y)];
        }

        public void SetItem(int x, int y, FieldItem item)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y));
            _field[GetIndex(x, y)] = item;
        }

        public static (int x, int y) GetNextLocation(int x, int y, Direction direction)
        {
            int dx = 0;
            int dy = 0;
            switch (direction)
            {
                case Direction.Up:
                    dy = -1;
                    break;
                case Direction.Right:
                    dx = 1;
                    break;
                case Direction.Down:
                    dy = 1;
                    break;
                case Direction.Left:
                    dx = -1;
                    break;
            }

            int tx = x + dx;
            int ty = y + dy;

            return (tx, ty);
        }


        private bool CanMove(int x, int y, Direction direction, bool skip)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y));

            (int tx, int ty) = GetNextLocation(x, y, direction);

            if (tx < 0 || tx >= Width || ty < 0 || ty >= Height)
                return false;

            bool isY = direction is Direction.Up or Direction.Down;

            FieldItem item = GetItem(x, y);

            if (item == FieldItem.Wall)
                return false;
            if (item == FieldItem.Nothing)
                return true;
            if (item == FieldItem.Box)
                return CanMove(tx, ty, direction, false);
            if (!isY && item is FieldItem.LeftBox or FieldItem.RightBox)
                return CanMove(tx, ty, direction, false);
            if (isY && item == FieldItem.LeftBox)
                return CanMove(tx, ty, direction, false) &&
                       (skip || CanMove(x + 1, y, direction, true));
            if (isY && item == FieldItem.RightBox)
                return CanMove(tx, ty, direction, false) &&
                       (skip || CanMove(x - 1, y, direction, true));
            throw new Exception("Unknown item");
        }

        public bool CanMove(int x, int y, Direction direction)
        {
            return CanMove(x, y, direction, false);
        }


        private bool Move(int x, int y, Direction direction, bool skip)
        {
            (int tx, int ty) = GetNextLocation(x, y, direction);

            if (tx < 0 || tx >= Width || ty < 0 || ty >= Height)
                return false;

            FieldItem item = GetItem(x, y);

            if (!CanMove(x, y, direction))
                return false;

            if (item == FieldItem.Nothing)
                return true;

            if (!Move(tx, ty, direction, false))
                return false;

            bool isY = direction is Direction.Up or Direction.Down;

            if (isY && item == FieldItem.LeftBox && !skip)
                Move(x + 1, y, direction, true);
            if (isY && item == FieldItem.RightBox && !skip)
                Move(x - 1, y, direction, true);

            SetItem(tx, ty, item);
            SetItem(x, y, FieldItem.Nothing);
            return true;
        }

        public bool Move(int x, int y, Direction direction)
        {
            return Move(x, y, direction, false);
        }

        public override string ToString()
        {
            return ToString([]);
        }

        public string ToString(params (int x, int y, char c)[] writes)
        {
            StringBuilder builder = new();
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    (int x, int y, char c)? overwrite = writes.FirstOrDefault(item => item.x == x && item.y == y);
                    if (overwrite.HasValue && overwrite.Value.c != '\0')
                    {
                        builder.Append(overwrite.Value.c);
                    }
                    else
                    {
                        switch (GetItem(x, y))
                        {
                            case FieldItem.Wall:
                                builder.Append('#');
                                break;
                            case FieldItem.Box:
                                builder.Append('O');
                                break;
                            case FieldItem.Nothing:
                                builder.Append('.');
                                break;
                            case FieldItem.LeftBox:
                                builder.Append('[');
                                break;
                            case FieldItem.RightBox:
                                builder.Append(']');
                                break;
                            default:
                                builder.Append('?');
                                break;
                        }
                    }
                }

                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}
