﻿﻿﻿﻿﻿﻿using MartianRobots.Common;

namespace MartianRobots.Commands
{
    public static class NavigationCommandFactory
    {
        public static ICommand CreateNavigationCommand(char commandChar)
        {
            return commandChar switch
            {
                Constants.Commands.TURN_LEFT => new TurnLeftCommand(),
                Constants.Commands.TURN_RIGHT => new TurnRightCommand(),
                Constants.Commands.MOVE_FORWARD => new MoveForwardCommand(),
                _ => throw new ArgumentException($"Invalid command character: '{commandChar}'. Valid commands are L (turn left), R (turn right), F (move forward)", nameof(commandChar))
            };
        }

        public static IEnumerable<ICommand> CreateNavigationCommands(string commandString)
        {
            if (string.IsNullOrEmpty(commandString))
                return Enumerable.Empty<ICommand>();

            return commandString.Select(CreateNavigationCommand);
        }
    }
}
