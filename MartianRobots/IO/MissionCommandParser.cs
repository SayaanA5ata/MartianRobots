﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using MartianRobots.Common;
using MartianRobots.Models;

namespace MartianRobots.IO
{
    public static class MissionCommandParser
    {
        public static MarsSurface ParseMarsSurface(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentException("Surface definition cannot be null or empty", nameof(line));

            // Remove BOM and trim whitespace
            line = line.Trim('\uFEFF').Trim();
            
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != Constants.Input.GRID_PARTS_COUNT)
                throw new ArgumentException("Surface definition must contain exactly two space-separated integers", nameof(line));

            if (!int.TryParse(parts[0], out var maxX) || !int.TryParse(parts[1], out var maxY))
                throw new ArgumentException("Surface dimensions must be valid integers", nameof(line));

            if (maxX < Constants.Grid.MIN_DIMENSION || maxX > Constants.Grid.MAX_DIMENSION)
                throw new ArgumentOutOfRangeException(nameof(maxX), $"Surface X dimension must be between {Constants.Grid.MIN_DIMENSION} and {Constants.Grid.MAX_DIMENSION}");

            if (maxY < Constants.Grid.MIN_DIMENSION || maxY > Constants.Grid.MAX_DIMENSION)
                throw new ArgumentOutOfRangeException(nameof(maxY), $"Surface Y dimension must be between {Constants.Grid.MIN_DIMENSION} and {Constants.Grid.MAX_DIMENSION}");

            return new MarsSurface(maxX, maxY);
        }

        public static (int x, int y, Orientation orientation) ParseExplorerPosition(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentException("Explorer position cannot be null or empty", nameof(line));

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != Constants.Input.ROBOT_POSITION_PARTS_COUNT)
                throw new ArgumentException("Explorer position must contain exactly three space-separated values (x y orientation)", nameof(line));

            if (!int.TryParse(parts[0], out var x) || !int.TryParse(parts[1], out var y))
                throw new ArgumentException("Explorer coordinates must be valid integers", nameof(line));

            if (x < Constants.Grid.MIN_DIMENSION || y < Constants.Grid.MIN_DIMENSION)
                throw new ArgumentException("Explorer coordinates must be non-negative", nameof(line));

            if (!Enum.TryParse(parts[2], out Orientation orientation))
                throw new ArgumentException($"Invalid orientation '{parts[2]}'. Valid orientations are: N, E, S, W", nameof(line));

            return (x, y, orientation);
        }

        public static string ParseNavigationCommands(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentException("Navigation commands cannot be null or empty", nameof(line));

            var commands = line.Trim().ToUpperInvariant();
            
            // Validate that all characters are valid commands
            foreach (char command in commands)
            {
                if (!Constants.Commands.VALID_COMMANDS.Contains(command))
                    throw new ArgumentException($"Invalid command '{command}'. Valid commands are: L (turn left), R (turn right), F (move forward)", nameof(line));
            }

            return commands;
        }
    }
}
