﻿using Xunit;
using MartianRobots.Commands;
using MartianRobots.Models;

namespace MartianRobots.Tests
{
    /// <summary>
    /// Tests for command creation and execution functionality.
    /// </summary>
    public class CommandTests
    {
        [Fact]
        public void CommandFactory_CreatesCorrectCommands()
        {
            // Arrange & Act
            var commands = NavigationCommandFactory.CreateNavigationCommands("LFR").ToList();

            // Assert
            Assert.Equal(3, commands.Count);
            Assert.IsType<TurnLeftCommand>(commands[0]);
            Assert.IsType<MoveForwardCommand>(commands[1]);
            Assert.IsType<TurnRightCommand>(commands[2]);
        }

        [Fact]
        public void CommandFactory_CreateCommand_ReturnsCorrectTypes()
        {
            // Act & Assert
            Assert.IsType<TurnLeftCommand>(NavigationCommandFactory.CreateNavigationCommand('L'));
            Assert.IsType<TurnRightCommand>(NavigationCommandFactory.CreateNavigationCommand('R'));
            Assert.IsType<MoveForwardCommand>(NavigationCommandFactory.CreateNavigationCommand('F'));
        }

        [Fact]
        public void CommandFactory_CreateCommand_ThrowsForInvalidCommand()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => NavigationCommandFactory.CreateNavigationCommand('X'));
            Assert.Contains("Invalid command character", exception.Message);
        }

        [Fact]
        public void CommandFactory_CreateCommands_HandlesEmptyString()
        {
            // Act
            var commands = NavigationCommandFactory.CreateNavigationCommands("");

            // Assert
            Assert.Empty(commands);
        }

        [Fact]
        public void MoveForwardCommand_ExecutesCorrectly()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);
            var robot = new MartianExplorer(2, 2, Orientation.N, grid);
            var command = new MoveForwardCommand();

            // Act
            command.Execute(robot);

            // Assert
            Assert.Equal(2, robot.X);
            Assert.Equal(3, robot.Y);
        }

        [Fact]
        public void TurnLeftCommand_ExecutesCorrectly()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);
            var robot = new MartianExplorer(2, 2, Orientation.N, grid);
            var command = new TurnLeftCommand();

            // Act
            command.Execute(robot);

            // Assert
            Assert.Equal(Orientation.W, robot.Orientation);
        }

        [Fact]
        public void TurnRightCommand_ExecutesCorrectly()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);
            var robot = new MartianExplorer(2, 2, Orientation.N, grid);
            var command = new TurnRightCommand();

            // Act
            command.Execute(robot);

            // Assert
            Assert.Equal(Orientation.E, robot.Orientation);
        }
    }
}
