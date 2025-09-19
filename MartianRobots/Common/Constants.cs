namespace MartianRobots.Common
{
    public static class Constants
    {
        public static class Grid
        {
            public const int MIN_DIMENSION = 0;
            public const int MAX_DIMENSION = 50;
        }

        public static class Commands
        {
            public const char TURN_LEFT = 'L';
            public const char TURN_RIGHT = 'R';
            public const char MOVE_FORWARD = 'F';
            public static readonly HashSet<char> VALID_COMMANDS = new() { TURN_LEFT, TURN_RIGHT, MOVE_FORWARD };
        }

        public static class Output
        {
            public const string LOST_INDICATOR = "LOST";
            public const string SEPARATOR = " ";
        }

        public static class Input
        {
            public const int MIN_INPUT_LINES = 3;
            public const int GRID_PARTS_COUNT = 2;
            public const int ROBOT_POSITION_PARTS_COUNT = 3;
        }
    }
}