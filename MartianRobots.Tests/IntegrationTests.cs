﻿﻿﻿using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MartianRobots.Tests
{
    /// <summary>
    /// Integration tests for the complete robot simulation system.
    /// </summary>
    public class IntegrationTests
    {
        [Fact]
        public void SampleInput_ProducesCorrectOutput()
        {
            // Arrange
            var inputLines = new List<string>
            {
                "5 3",
                "1 1 E",
                "RFRFRFRF",
                "3 2 N",
                "FRRFLLFFRRFLL",
                "0 3 W",
                "LLFFFLFLFL"
            };

            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            Program.ExecuteMissionPlan(inputLines);

            // Assert
            var output = sw.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            Assert.Equal(3, output.Length);
            Assert.Equal("1 1 E", output[0]);
            Assert.Equal("3 3 N LOST", output[1]);
            Assert.Equal("2 3 S", output[2]);
        }

        [Fact]
        public void SingleRobot_SimpleMovement_ProducesCorrectOutput()
        {
            // Arrange
            var inputLines = new List<string>
            {
                "3 3",
                "0 0 N",
                "FRF"
            };

            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            Program.ExecuteMissionPlan(inputLines);

            // Assert
            var output = sw.ToString().Trim();
            Assert.Equal("1 1 E", output);
        }

        [Fact]
        public void RobotFallsOffGrid_ProducesLostOutput()
        {
            // Arrange
            var inputLines = new List<string>
            {
                "1 1",
                "1 1 N",
                "F"
            };

            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            Program.ExecuteMissionPlan(inputLines);

            // Assert
            var output = sw.ToString().Trim();
            Assert.Equal("1 1 N LOST", output);
        }

        [Fact]
        public void ScentMarker_PreventsSecondRobotFromFalling()
        {
            // Arrange
            var inputLines = new List<string>
            {
                "2 2",
                "2 2 N",
                "F", // First robot falls and leaves scent
                "2 2 N",
                "F" // Second robot should not fall
            };

            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            Program.ExecuteMissionPlan(inputLines);

            // Assert
            var output = sw.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, output.Length);
            Assert.Equal("2 2 N LOST", output[0]);
            Assert.Equal("2 2 N", output[1]); // Second robot should not be lost
        }

        [Fact]
        public void EmptyCommands_ThrowsException()
        {
            // Arrange
            var inputLines = new List<string>
            {
                "5 5",
                "2 3 W",
                "" // No commands
            };

            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            Program.ExecuteMissionPlan(inputLines);

            // Assert - Should output error message for this robot
            var output = sw.ToString();
            Assert.Contains("Error processing robot", output);
        }

        [Fact]
        public void InvalidInput_ThrowsException()
        {
            // Arrange
            var inputLines = new List<string>
            {
                "invalid grid"
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Program.ExecuteMissionPlan(inputLines));
        }

        [Fact]
        public void RobotOutsideGrid_ThrowsException()
        {
            // Arrange
            var inputLines = new List<string>
            {
                "3 3",
                "5 5 N", // Robot position outside grid
                "F"
            };

            using var sw = new StringWriter();
            using var errorSw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(errorSw);

            // Act
            Program.ExecuteMissionPlan(inputLines);

            // Assert - Should output error message
            var output = sw.ToString();
            Assert.Contains("Error processing robot", output);
        }
    }
}
