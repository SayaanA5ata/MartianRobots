using Xunit;
using MartianRobots.Models;

namespace MartianRobots.Tests
{
    /// <summary>
    /// Tests for Grid class functionality.
    /// </summary>
    public class GridTests
    {
        [Fact]
        public void Grid_Constructor_InitializesCorrectly()
        {
            // Act
            var grid = new MarsSurface(5, 3);

            // Assert
            Assert.Equal(5, grid.MaxX);
            Assert.Equal(3, grid.MaxY);
            Assert.Equal(24, grid.Area); // (5+1) * (3+1) = 6 * 4 = 24
            Assert.Equal(0, grid.GetDangerBeaconCount());
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, -1)]
        public void Grid_Constructor_ThrowsForNegativeDimensions(int maxX, int maxY)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new MarsSurface(maxX, maxY));
        }

        [Theory]
        [InlineData(0, 0, true)]
        [InlineData(2, 3, true)]
        [InlineData(5, 3, true)]
        [InlineData(6, 3, false)]
        [InlineData(5, 4, false)]
        [InlineData(-1, 0, false)]
        [InlineData(0, -1, false)]
        public void Grid_IsPositionValid_ReturnsCorrectResult(int x, int y, bool expected)
        {
            // Arrange
            var grid = new MarsSurface(5, 3);

            // Act
            var result = grid.IsWithinExplorationBounds(x, y);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Grid_ScentMarkers_WorkCorrectly()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);

            // Act & Assert - Initially no scent markers
            Assert.False(grid.HasDangerBeacon(2, 3));
            Assert.Equal(0, grid.GetDangerBeaconCount());

            // Add scent marker
            grid.PlaceDangerBeacon(2, 3);
            Assert.True(grid.HasDangerBeacon(2, 3));
            Assert.Equal(1, grid.GetDangerBeaconCount());

            // Different position should not have scent marker
            Assert.False(grid.HasDangerBeacon(3, 2));

            // Add multiple scent markers
            grid.PlaceDangerBeacon(3, 2);
            grid.PlaceDangerBeacon(1, 1);
            Assert.Equal(3, grid.GetDangerBeaconCount());
        }

        [Fact]
        public void Grid_ScentMarkers_NoDuplicates()
        {
            // Arrange
            var grid = new MarsSurface(5, 5);

            // Act
            grid.PlaceDangerBeacon(2, 3);
            grid.PlaceDangerBeacon(2, 3); // Add same marker again

            // Assert
            Assert.True(grid.HasDangerBeacon(2, 3));
            Assert.Equal(1, grid.GetDangerBeaconCount()); // Should still be 1
        }

        [Fact]
        public void Grid_ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var grid = new MarsSurface(5, 3);
            grid.PlaceDangerBeacon(1, 1);
            grid.PlaceDangerBeacon(2, 2);

            // Act
            var result = grid.ToString();

            // Assert
            Assert.Equal("Grid(5x3, 2 scent markers)", result);
        }
    }
}