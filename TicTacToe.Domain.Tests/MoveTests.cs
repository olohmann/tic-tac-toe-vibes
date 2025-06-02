using TicTacToe.Domain;

namespace TicTacToe.Domain.Tests;

/// <summary>
/// Tests for the Move record.
/// </summary>
public class MoveTests
{
    [Fact]
    public void Move_Constructor_ShouldSetAllProperties()
    {
        // Act
        var move = new Move(1, 2, Player.X, 5);

        // Assert
        Assert.Equal(1, move.Row);
        Assert.Equal(2, move.Col);
        Assert.Equal(Player.X, move.Player);
        Assert.Equal(5, move.SequenceNumber);
    }

    [Fact]
    public void Move_Equality_ShouldWorkCorrectly()
    {
        // Arrange
        var move1 = new Move(1, 2, Player.X, 5);
        var move2 = new Move(1, 2, Player.X, 5);
        var move3 = new Move(1, 2, Player.O, 5);

        // Act & Assert
        Assert.Equal(move1, move2);
        Assert.NotEqual(move1, move3);
    }
}
