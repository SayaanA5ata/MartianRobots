﻿﻿﻿using MartianRobots.Models;

namespace MartianRobots.Commands
{
    public class TurnLeftCommand : ICommand
    {
        public void Execute(MartianExplorer explorer) => explorer.TurnLeft();
    }
}
