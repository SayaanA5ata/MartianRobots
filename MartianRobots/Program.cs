﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using MartianRobots.Commands;
using MartianRobots.Common;
using MartianRobots.IO;
using MartianRobots.Models;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var inputLines = ReadAllInputLines();
            ExecuteMissionPlan(inputLines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Environment.Exit(1);
        }
    }

    private static List<string> ReadAllInputLines()
    {
        var inputLines = new List<string>();
        string? line;

        while ((line = Console.ReadLine()) != null)
        {
            inputLines.Add(line);
        }

        return inputLines;
    }

    public static void ExecuteMissionPlan(List<string> inputLines)
    {
        ValidateInputFormat(inputLines);

        try
        {
            var surface = MissionCommandParser.ParseMarsSurface(inputLines[0]);
            ProcessExplorerInstructions(inputLines, surface);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Failed to process input: {ex.Message}", ex);
        }
    }

    private static void ValidateInputFormat(List<string> inputLines)
    {
        if (inputLines == null || inputLines.Count == 0)
            throw new ArgumentException("Input cannot be empty");

        if (inputLines.Count < Constants.Input.MIN_INPUT_LINES)
            throw new ArgumentException("Input must contain at least a surface definition and one explorer with commands");

        if ((inputLines.Count - 1) % 2 != 0)
            throw new ArgumentException("Each explorer must have both position and command lines");
    }

    private static void ProcessExplorerInstructions(List<string> inputLines, MarsSurface surface)
    {
        for (int i = 1; i < inputLines.Count; i += 2)
        {
            if (i + 1 >= inputLines.Count) break;

            try
            {
                var (x, y, orientation) = MissionCommandParser.ParseExplorerPosition(inputLines[i]);
                var commandString = MissionCommandParser.ParseNavigationCommands(inputLines[i + 1]);

                // Validate explorer starting position is within surface bounds
                if (!surface.IsWithinExplorationBounds(x, y))
                {
                    throw new ArgumentException($"Explorer starting position ({x}, {y}) is outside surface bounds (0,0) to ({surface.MaxX}, {surface.MaxY})");
                }

                var explorer = new MartianExplorer(x, y, orientation, surface);
                var commands = NavigationCommandFactory.CreateNavigationCommands(commandString);

                explorer.PerformMission(commands);
                Console.WriteLine(ExplorationReporter.GenerateExplorationReport(explorer));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing explorer at line {i + 1}: {ex.Message}");
            }
        }
    }
}