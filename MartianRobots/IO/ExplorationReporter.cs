﻿﻿using MartianRobots.Models;

namespace MartianRobots.IO
{
    public static class ExplorationReporter
    {
        public static string GenerateExplorationReport(MartianExplorer explorer)
        {
            if (explorer == null)
                throw new ArgumentNullException(nameof(explorer));

            return explorer.IsLost
                ? $"{explorer.X} {explorer.Y} {explorer.Orientation} LOST"
                : $"{explorer.X} {explorer.Y} {explorer.Orientation}";
        }
    }
}
