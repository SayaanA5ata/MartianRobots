using Xunit;
using MartianRobots.IO;
using MartianRobots.Models;

namespace MartianRobots.Tests
{
    /// <summary>
    /// Tests for InputParser class functionality.
    /// </summary>
    public class InputParserTests
    {
        [Fact]
        public void ParseGrid_ValidInput_ReturnsCorrectGrid()
        {
            // Act
            var grid = MissionCommandParser.ParseMarsSurface("5 3");

            // Assert
            Assert.Equal(5, grid.MaxX);
            Assert.Equal(3, grid.MaxY);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ParseGrid_InvalidInput_ThrowsException(string input)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => MissionCommandParser.ParseMarsSurface(input));
        }

        [Theory]
        [InlineData("5")]
        [InlineData("5 3 2")]
        [InlineData("5  3  2")]
        public void ParseGrid_WrongNumberOfParts_ThrowsException(string input)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => MissionCommandParser.ParseMarsSurface(input));
            Assert.Contains("exactly two space-separated integers", exception.Message);
        }

        [Theory]
        [InlineData("abc def")]
        [InlineData("5 abc")]
        [InlineData("abc 3")]
        public void ParseGrid_NonIntegerValues_ThrowsException(string input)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => MissionCommandParser.ParseMarsSurface(input));
            Assert.Contains("valid integers", exception.Message);
        }

        [Theory]
        [InlineData("-1 3")]
        [InlineData("5 -1")]
        [InlineData("51 3")]
        [InlineData("5 51")]
        public void ParseGrid_OutOfRangeValues_ThrowsException(string input)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => MissionCommandParser.ParseMarsSurface(input));
        }

        [Fact]
        public void ParseRobotPosition_ValidInput_ReturnsCorrectValues()
        {
            // Act
            var (x, y, orientation) = MissionCommandParser.ParseExplorerPosition("1 2 E");

            // Assert
            Assert.Equal(1, x);
            Assert.Equal(2, y);
            Assert.Equal(Orientation.E, orientation);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ParseRobotPosition_InvalidInput_ThrowsException(string input)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => MissionCommandParser.ParseExplorerPosition(input));
        }

        [Theory]
        [InlineData("1 2")]
        [InlineData("1 2 E S")]
        public void ParseRobotPosition_WrongNumberOfParts_ThrowsException(string input)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => MissionCommandParser.ParseExplorerPosition(input));
            Assert.Contains("exactly three space-separated values", exception.Message);
        }

        [Theory]
        [InlineData("abc 2 E")]
        [InlineData("1 abc E")]
        public void ParseRobotPosition_NonIntegerCoordinates_ThrowsException(string input)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => MissionCommandParser.ParseExplorerPosition(input));
            Assert.Contains("valid integers", exception.Message);
        }

        [Theory]
        [InlineData("-1 2 E")]
        [InlineData("1 -1 E")]
        public void ParseRobotPosition_NegativeCoordinates_ThrowsException(string input)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => MissionCommandParser.ParseExplorerPosition(input));
            Assert.Contains("non-negative", exception.Message);
        }

        [Theory]
        [InlineData("1 2 X")]
        [InlineData("1 2 North")]
        [InlineData("1 2 Z")]
        public void ParseRobotPosition_InvalidOrientation_ThrowsException(string input)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => MissionCommandParser.ParseExplorerPosition(input));
            Assert.Contains("Invalid orientation", exception.Message);
        }

        [Fact]
        public void ParseRobotCommands_ValidInput_ReturnsCorrectCommands()
        {
            // Act
            var result = MissionCommandParser.ParseNavigationCommands("LFR");

            // Assert
            Assert.Equal("LFR", result);
        }

        [Fact]
        public void ParseRobotCommands_LowercaseInput_ReturnsUppercase()
        {
            // Act
            var result = MissionCommandParser.ParseNavigationCommands("lfr");

            // Assert
            Assert.Equal("LFR", result);
        }

        [Fact]
        public void ParseRobotCommands_WithWhitespace_TrimsCorrectly()
        {
            // Act
            var result = MissionCommandParser.ParseNavigationCommands("  LFR  ");

            // Assert
            Assert.Equal("LFR", result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ParseRobotCommands_InvalidInput_ThrowsException(string input)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => MissionCommandParser.ParseNavigationCommands(input));
        }

        [Theory]
        [InlineData("LFX")]
        [InlineData("ABC")]
        [InlineData("L123")]
        public void ParseRobotCommands_InvalidCharacters_ThrowsException(string input)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => MissionCommandParser.ParseNavigationCommands(input));
            Assert.Contains("Invalid command", exception.Message);
        }
    }
}