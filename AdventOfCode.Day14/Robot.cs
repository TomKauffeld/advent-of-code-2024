using AdventOfCode.Core;

namespace AdventOfCode.Day14
{
    internal class Robot
    {
        public Vector2L StartingPosition { get; }
        public Vector2L Position { get; set; }
        public Vector2L Velocity { get; set; }

        public Robot(Vector2L position, Vector2L velocity)
        {
            StartingPosition = position;
            Position = position;
            Velocity = velocity;
        }


    }
}
