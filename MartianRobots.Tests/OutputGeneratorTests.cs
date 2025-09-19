using Xunit;
using MartianRobots.IO;
using MartianRobots.Models;

namespace MartianRobots.Tests
{
    /// <summary>
    /// Tests for ExplorationReporter class functionality.
    /// </summary>
    public class ExplorationReporterTests
    {
        [Fact]
        public void GenerateExplorationReport_ExplorerNotLost_ReturnsCorrectFormat()
        {
            // Arrange
            var surface = new MarsSurface(5, 5);
            var explorer = new MartianExplorer(1, 2, Orientation.E, surface);

            // Act
            var result = ExplorationReporter.GenerateExplorationReport(explorer);

            // Assert
            Assert.Equal("1 2 E", result);
        }

        [Fact]
        public void GenerateExplorationReport_ExplorerLost_ReturnsCorrectFormat()
        {
            // Arrange
            var surface = new MarsSurface(2, 2);
            var explorer = new MartianExplorer(2, 2, Orientation.N, surface);
            explorer.AdvanceForward(); // Make explorer lost

            // Act
            var result = ExplorationReporter.GenerateExplorationReport(explorer);

            // Assert
            Assert.Equal("2 2 N LOST", result);
        }

        [Theory]
        [InlineData(0, 0, Orientation.N, "0 0 N")]
        [InlineData(5, 3, Orientation.S, "5 3 S")]
        [InlineData(2, 4, Orientation.W, "2 4 W")]
        [InlineData(1, 1, Orientation.E, "1 1 E")]
        public void GenerateExplorationReport_VariousPositions_ReturnsCorrectFormat(int x, int y, Orientation orientation, string expected)
        {
            // Arrange
            var surface = new MarsSurface(10, 10);
            var explorer = new MartianExplorer(x, y, orientation, surface);

            // Act
            var result = ExplorationReporter.GenerateExplorationReport(explorer);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GenerateExplorationReport_NullExplorer_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ExplorationReporter.GenerateExplorationReport(null!));
        }
    }
}