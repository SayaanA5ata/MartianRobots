﻿﻿﻿﻿namespace MartianRobots.Models
{
    public class MarsSurface
    {
        public int MaxX { get; }

        public int MaxY { get; }

        public int Area => (MaxX + 1) * (MaxY + 1);

        private readonly HashSet<(int X, int Y)> _dangerBeacons = new();

        public MarsSurface(int maxX, int maxY)
        {
            if (maxX < 0)
                throw new ArgumentException("Maximum X coordinate cannot be negative", nameof(maxX));
            if (maxY < 0)
                throw new ArgumentException("Maximum Y coordinate cannot be negative", nameof(maxY));

            MaxX = maxX;
            MaxY = maxY;
        }

        public bool IsWithinExplorationBounds(int x, int y)
        {
            return x >= 0 && x <= MaxX && y >= 0 && y <= MaxY;
        }

        public void PlaceDangerBeacon(int x, int y)
        {
            _dangerBeacons.Add((x, y));
        }

        public bool HasDangerBeacon(int x, int y)
        {
            return _dangerBeacons.Contains((x, y));
        }

        public int GetDangerBeaconCount()
        {
            return _dangerBeacons.Count;
        }

        public override string ToString()
        {
            return $"MarsSurface({MaxX}x{MaxY}, {GetDangerBeaconCount()} danger beacons)";
        }
    }
}
