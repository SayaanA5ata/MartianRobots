﻿﻿using MartianRobots.Models;

namespace MartianRobots.Commands
{
    public interface ICommand
    {
        void Execute(MartianExplorer explorer);
    }
}
