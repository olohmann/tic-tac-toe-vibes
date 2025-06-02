using TicTacToe.Domain;

namespace TicTacToe.Domain.Tests;

/// <summary>
/// Tests for the Player enum.
/// </summary>
public class PlayerTests
{
    [Fact]
    public void Player_ShouldHaveXAndOValues()
    {
        // Act & Assert
        Assert.Equal(0, (int)Player.X);
        Assert.Equal(1, (int)Player.O);
    }
}
