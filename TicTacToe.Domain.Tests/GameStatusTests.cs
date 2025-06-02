using TicTacToe.Domain;

namespace TicTacToe.Domain.Tests;

/// <summary>
/// Tests for the GameStatus enum.
/// </summary>
public class GameStatusTests
{
    [Fact]
    public void GameStatus_ShouldHaveCorrectValues()
    {
        // Act & Assert
        Assert.Equal(0, (int)GameStatus.InProgress);
        Assert.Equal(1, (int)GameStatus.X_Won);
        Assert.Equal(2, (int)GameStatus.O_Won);
        Assert.Equal(3, (int)GameStatus.Draw);
    }
}
