﻿﻿using MartianRobots.Models;

namespace MartianRobots.Commands
{
    public class TurnRightCommand : ICommand
    {
        public void Execute(MartianExplorer explorer) => explorer.TurnRight();
    }
}
