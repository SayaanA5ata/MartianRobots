﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using MartianRobots.Commands;

namespace MartianRobots.Models
{
    public class MartianExplorer
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public Orientation Orientation { get; private set; }

        public bool IsLost { get; private set; }

        public (int X, int Y) Position => (X, Y);

        private readonly MarsSurface _surface;

        public MartianExplorer(int x, int y, Orientation orientation, MarsSurface surface)
        {
            _surface = surface ?? throw new ArgumentNullException(nameof(surface));
            
            if (!_surface.IsWithinExplorationBounds(x, y))
                throw new ArgumentException($"Initial position ({x}, {y}) is outside surface bounds");
            
            X = x;
            Y = y;
            Orientation = orientation;
            IsLost = false;
        }

        public void PerformMission(IEnumerable<ICommand> commands)
        {
            if (commands == null)
                throw new ArgumentNullException(nameof(commands));

            foreach (var command in commands)
            {
                if (IsLost) break;
                
                if (command == null)
                    throw new ArgumentException("Command cannot be null");
                    
                command.Execute(this);
            }
        }

        public void AdvanceForward()
        {
            if (IsLost) return;

            var (newX, newY) = CalculateNextPosition();

            if (!_surface.IsWithinExplorationBounds(newX, newY))
            {
                HandleOffSurfaceMovement();
                return;
            }

            X = newX;
            Y = newY;
        }

        private (int X, int Y) CalculateNextPosition()
        {
            return Orientation switch
            {
                Orientation.N => (X, Y + 1),
                Orientation.E => (X + 1, Y),
                Orientation.S => (X, Y - 1),
                Orientation.W => (X - 1, Y),
                _ => throw new InvalidOperationException($"Unknown orientation: {Orientation}")
            };
        }

        private void HandleOffSurfaceMovement()
        {
            if (!_surface.HasDangerBeacon(X, Y))
            {
                _surface.PlaceDangerBeacon(X, Y);
                IsLost = true;
            }
        }

        public void TurnLeft()
        {
            if (IsLost) return;

            Orientation = Orientation switch
            {
                Orientation.N => Orientation.W,
                Orientation.W => Orientation.S,
                Orientation.S => Orientation.E,
                Orientation.E => Orientation.N,
                _ => throw new InvalidOperationException($"Unknown orientation: {Orientation}")
            };
        }

        public void TurnRight()
        {
            if (IsLost) return;

            Orientation = Orientation switch
            {
                Orientation.N => Orientation.E,
                Orientation.E => Orientation.S,
                Orientation.S => Orientation.W,
                Orientation.W => Orientation.N,
                _ => throw new InvalidOperationException($"Unknown orientation: {Orientation}")
            };
        }

        public override string ToString()
        {
            var status = IsLost ? " LOST" : string.Empty;
            return $"{X} {Y} {Orientation}{status}";
        }
    }
}
