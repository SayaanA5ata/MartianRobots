﻿﻿﻿using MartianRobots.Models;

namespace MartianRobots.Commands
{
    public class MoveForwardCommand : ICommand
    {
        public void Execute(MartianExplorer explorer) => explorer.AdvanceForward();
    }
}
