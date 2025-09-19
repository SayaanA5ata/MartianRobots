using MartianRobots.Models;
using MartianRobots.Commands;
using Xunit;

namespace MartianRobots.Tests
{
    /// <summary>
    /// Tests for Robot class functionality.
    /// </summary>
    public class RobotTests
    {
        [Fact]
        public void Robot_Constructor_InitializesCorrectly()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);

            // Act
            var robot = new MartianExplorer(1, 2, Orientation.E, grid);

            // Assert
            Assert.Equal(1, robot.X);
            Assert.Equal(2, robot.Y);
            Assert.Equal(Orientation.E, robot.Orientation);
            Assert.False(robot.IsLost);
            Assert.Equal((1, 2), robot.Position);
        }

        [Fact]
        public void Robot_Constructor_ThrowsForNullGrid()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MartianExplorer(1, 1, Orientation.N, null!));
        }

        [Fact]
        public void Robot_Constructor_ThrowsForInvalidPosition()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new MartianExplorer(6, 6, Orientation.N, grid));
        }

        [Fact]
        public void Robot_TurnsLeft_Correctly()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);
            var robot = new MartianExplorer(1, 1, Orientation.N, grid);

            // Act & Assert
            robot.TurnLeft();
            Assert.Equal(Orientation.W, robot.Orientation);

            robot.TurnLeft();
            Assert.Equal(Orientation.S, robot.Orientation);

            robot.TurnLeft();
            Assert.Equal(Orientation.E, robot.Orientation);

            robot.TurnLeft();
            Assert.Equal(Orientation.N, robot.Orientation);
        }

        [Fact]
        public void Robot_TurnsRight_Correctly()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);
            var robot = new MartianExplorer(1, 1, Orientation.N, grid);

            // Act & Assert
            robot.TurnRight();
            Assert.Equal(Orientation.E, robot.Orientation);

            robot.TurnRight();
            Assert.Equal(Orientation.S, robot.Orientation);

            robot.TurnRight();
            Assert.Equal(Orientation.W, robot.Orientation);

            robot.TurnRight();
            Assert.Equal(Orientation.N, robot.Orientation);
        }

        [Theory]
        [InlineData(Orientation.N, 1, 2)]
        [InlineData(Orientation.E, 2, 1)]
        [InlineData(Orientation.S, 1, 0)]
        [InlineData(Orientation.W, 0, 1)]
        public void Robot_MovesForward_Correctly(Orientation orientation, int expectedX, int expectedY)
        {
            // Arrange
            var grid = new MarsSurface(5, 5);
            var robot = new MartianExplorer(1, 1, orientation, grid);

            // Act
            robot.AdvanceForward();

            // Assert
            Assert.Equal(expectedX, robot.X);
            Assert.Equal(expectedY, robot.Y);
            Assert.False(robot.IsLost);
        }

        [Fact]
        public void Robot_FallsOffGrid_AndLeavesScent()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);
            var robot = new MartianExplorer(5, 5, Orientation.N, grid);

            // Act
            robot.AdvanceForward();

            // Assert
            Assert.True(robot.IsLost);
            Assert.True(grid.HasDangerBeacon(5, 5));
            Assert.Equal(5, robot.X); // Position doesn't change when falling off
            Assert.Equal(5, robot.Y);
        }

        [Fact]
        public void Robot_IgnoresMove_WhenScentPresent()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);
            grid.PlaceDangerBeacon(5, 5);
            var robot = new MartianExplorer(5, 5, Orientation.N, grid);

            // Act
            robot.AdvanceForward();

            // Assert
            Assert.False(robot.IsLost);
            Assert.Equal(5, robot.X);
            Assert.Equal(5, robot.Y);
        }

        [Fact]
        public void Robot_IgnoresCommands_WhenLost()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);
            var robot = new MartianExplorer(5, 5, Orientation.N, grid);
            
            // Make robot lost
            robot.AdvanceForward();
            Assert.True(robot.IsLost);

            var originalX = robot.X;
            var originalY = robot.Y;
            var originalOrientation = robot.Orientation;

            // Act
            robot.TurnLeft();
            robot.TurnRight();
            robot.AdvanceForward();

            // Assert
            Assert.Equal(originalX, robot.X);
            Assert.Equal(originalY, robot.Y);
            Assert.Equal(originalOrientation, robot.Orientation);
        }

        [Fact]
        public void Robot_ExecuteCommands_StopsWhenLost()
        {
            // Arrange
            var grid = new MarsSurface(1, 1);
            var robot = new MartianExplorer(1, 1, Orientation.N, grid);
            var commands = new List<ICommand>
            {
                new MoveForwardCommand(), // This will make robot lost
                new TurnLeftCommand(),    // This should not execute
                new MoveForwardCommand()  // This should not execute
            };

            // Act
            robot.PerformMission(commands);

            // Assert
            Assert.True(robot.IsLost);
            Assert.Equal(Orientation.N, robot.Orientation); // Should not have turned
        }

        [Fact]
        public void Robot_ExecuteCommands_ThrowsForNullCommands()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);
            var robot = new MartianExplorer(1, 1, Orientation.N, grid);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => robot.PerformMission(null!));
        }

        [Fact]
        public void Robot_ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);
            var robot = new MartianExplorer(2, 3, Orientation.E, grid);

            // Act
            var result = robot.ToString();

            // Assert
            Assert.Equal("2 3 E", result);
        }

        [Fact]
        public void Robot_ToString_IncludesLostWhenRobotIsLost()
        {
            // Arrange
            var grid = new MarsSurface(2, 2);
            var robot = new MartianExplorer(2, 2, Orientation.N, grid);
            robot.AdvanceForward(); // Make robot lost

            // Act
            var result = robot.ToString();

            // Assert
            Assert.Equal("2 2 N LOST", result);
        }
    }
}