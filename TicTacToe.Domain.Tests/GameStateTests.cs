using TicTacToe.Domain;

namespace TicTacToe.Domain.Tests;

/// <summary>
/// Tests for the GameState class.
/// </summary>
public class GameStateTests
{
    [Fact]
    public void GameState_Constructor_ShouldInitializeCorrectly()
    {
        // Act
        var gameState = new GameState();

        // Assert
        Assert.Equal(Player.X, gameState.CurrentPlayer);
        Assert.Equal(GameStatus.InProgress, gameState.Status);
        Assert.Empty(gameState.MoveHistory);
        
        // Check board is empty
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                Assert.Equal(' ', gameState.GetCell(row, col));
            }
        }
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(3, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 3)]
    public void GetCell_WithInvalidCoordinates_ShouldThrowArgumentOutOfRangeException(int row, int col)
    {
        // Arrange
        var gameState = new GameState();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => gameState.GetCell(row, col));
    }

    [Fact]
    public void GetBoard_ShouldReturnCopyOfBoard()
    {
        // Arrange
        var gameState = new GameState();
        gameState.TryMakeMove(0, 0);

        // Act
        var board = gameState.GetBoard();
        board[1, 1] = 'Z'; // Modify the copy

        // Assert
        Assert.Equal('X', board[0, 0]); // Copy has the move
        Assert.Equal(' ', gameState.GetCell(1, 1)); // Original is unchanged
    }

    [Fact]
    public void TryMakeMove_ValidMove_ShouldReturnTrueAndUpdateState()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        var result = gameState.TryMakeMove(1, 1);

        // Assert
        Assert.True(result);
        Assert.Equal('X', gameState.GetCell(1, 1));
        Assert.Equal(Player.O, gameState.CurrentPlayer);
        Assert.Single(gameState.MoveHistory);
        
        var move = gameState.MoveHistory[0];
        Assert.Equal(1, move.Row);
        Assert.Equal(1, move.Col);
        Assert.Equal(Player.X, move.Player);
        Assert.Equal(1, move.SequenceNumber);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(3, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 3)]
    public void TryMakeMove_InvalidCoordinates_ShouldReturnFalse(int row, int col)
    {
        // Arrange
        var gameState = new GameState();

        // Act
        var result = gameState.TryMakeMove(row, col);

        // Assert
        Assert.False(result);
        Assert.Equal(Player.X, gameState.CurrentPlayer);
        Assert.Empty(gameState.MoveHistory);
    }

    [Fact]
    public void TryMakeMove_OccupiedCell_ShouldReturnFalse()
    {
        // Arrange
        var gameState = new GameState();
        gameState.TryMakeMove(1, 1);

        // Act
        var result = gameState.TryMakeMove(1, 1);

        // Assert
        Assert.False(result);
        Assert.Equal(Player.O, gameState.CurrentPlayer);
        Assert.Single(gameState.MoveHistory);
    }

    [Fact]
    public void TryMakeMove_GameAlreadyWon_ShouldReturnFalse()
    {
        // Arrange
        var gameState = new GameState();
        // Create winning condition for X
        gameState.TryMakeMove(0, 0); // X
        gameState.TryMakeMove(1, 0); // O
        gameState.TryMakeMove(0, 1); // X
        gameState.TryMakeMove(1, 1); // O
        gameState.TryMakeMove(0, 2); // X wins

        // Act
        var result = gameState.TryMakeMove(2, 2);

        // Assert
        Assert.False(result);
        Assert.Equal(GameStatus.X_Won, gameState.Status);
    }

    [Fact]
    public void TryMakeMove_XWinsHorizontalTopRow_ShouldSetCorrectStatus()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.TryMakeMove(0, 0); // X
        gameState.TryMakeMove(1, 0); // O
        gameState.TryMakeMove(0, 1); // X
        gameState.TryMakeMove(1, 1); // O
        gameState.TryMakeMove(0, 2); // X wins

        // Assert
        Assert.Equal(GameStatus.X_Won, gameState.Status);
        Assert.Equal(Player.X, gameState.CurrentPlayer); // Current player doesn't change when game ends
    }

    [Fact]
    public void TryMakeMove_XWinsHorizontalMiddleRow_ShouldSetCorrectStatus()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.TryMakeMove(1, 0); // X
        gameState.TryMakeMove(0, 0); // O
        gameState.TryMakeMove(1, 1); // X
        gameState.TryMakeMove(0, 1); // O
        gameState.TryMakeMove(1, 2); // X wins

        // Assert
        Assert.Equal(GameStatus.X_Won, gameState.Status);
    }

    [Fact]
    public void TryMakeMove_XWinsHorizontalBottomRow_ShouldSetCorrectStatus()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.TryMakeMove(2, 0); // X
        gameState.TryMakeMove(0, 0); // O
        gameState.TryMakeMove(2, 1); // X
        gameState.TryMakeMove(0, 1); // O
        gameState.TryMakeMove(2, 2); // X wins

        // Assert
        Assert.Equal(GameStatus.X_Won, gameState.Status);
    }

    [Fact]
    public void TryMakeMove_XWinsVerticalLeftColumn_ShouldSetCorrectStatus()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.TryMakeMove(0, 0); // X
        gameState.TryMakeMove(0, 1); // O
        gameState.TryMakeMove(1, 0); // X
        gameState.TryMakeMove(0, 2); // O
        gameState.TryMakeMove(2, 0); // X wins

        // Assert
        Assert.Equal(GameStatus.X_Won, gameState.Status);
    }

    [Fact]
    public void TryMakeMove_XWinsVerticalMiddleColumn_ShouldSetCorrectStatus()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.TryMakeMove(0, 1); // X
        gameState.TryMakeMove(0, 0); // O
        gameState.TryMakeMove(1, 1); // X
        gameState.TryMakeMove(0, 2); // O
        gameState.TryMakeMove(2, 1); // X wins

        // Assert
        Assert.Equal(GameStatus.X_Won, gameState.Status);
    }

    [Fact]
    public void TryMakeMove_XWinsVerticalRightColumn_ShouldSetCorrectStatus()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.TryMakeMove(0, 2); // X
        gameState.TryMakeMove(0, 0); // O
        gameState.TryMakeMove(1, 2); // X
        gameState.TryMakeMove(0, 1); // O
        gameState.TryMakeMove(2, 2); // X wins

        // Assert
        Assert.Equal(GameStatus.X_Won, gameState.Status);
    }

    [Fact]
    public void TryMakeMove_XWinsDiagonalTopLeftToBottomRight_ShouldSetCorrectStatus()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.TryMakeMove(0, 0); // X
        gameState.TryMakeMove(0, 1); // O
        gameState.TryMakeMove(1, 1); // X
        gameState.TryMakeMove(0, 2); // O
        gameState.TryMakeMove(2, 2); // X wins

        // Assert
        Assert.Equal(GameStatus.X_Won, gameState.Status);
    }

    [Fact]
    public void TryMakeMove_XWinsDiagonalTopRightToBottomLeft_ShouldSetCorrectStatus()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.TryMakeMove(0, 2); // X
        gameState.TryMakeMove(0, 0); // O
        gameState.TryMakeMove(1, 1); // X
        gameState.TryMakeMove(0, 1); // O
        gameState.TryMakeMove(2, 0); // X wins

        // Assert
        Assert.Equal(GameStatus.X_Won, gameState.Status);
    }

    [Fact]
    public void TryMakeMove_OWins_ShouldSetCorrectStatus()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.TryMakeMove(0, 0); // X
        gameState.TryMakeMove(1, 0); // O
        gameState.TryMakeMove(0, 1); // X
        gameState.TryMakeMove(1, 1); // O
        gameState.TryMakeMove(2, 2); // X
        gameState.TryMakeMove(1, 2); // O wins

        // Assert
        Assert.Equal(GameStatus.O_Won, gameState.Status);
        Assert.Equal(Player.O, gameState.CurrentPlayer);
    }

    [Fact]
    public void TryMakeMove_BoardFull_ShouldSetDrawStatus()
    {
        // Arrange
        var gameState = new GameState();

        // Act - Create a draw scenario
        gameState.TryMakeMove(0, 0); // X
        gameState.TryMakeMove(0, 1); // O
        gameState.TryMakeMove(0, 2); // X
        gameState.TryMakeMove(1, 0); // O
        gameState.TryMakeMove(1, 1); // X
        gameState.TryMakeMove(2, 0); // O
        gameState.TryMakeMove(1, 2); // X
        gameState.TryMakeMove(2, 2); // O
        gameState.TryMakeMove(2, 1); // X - Board full, no winner

        // Assert
        Assert.Equal(GameStatus.Draw, gameState.Status);
    }

    [Fact]
    public void TryMakeMove_GameInDraw_ShouldReturnFalse()
    {
        // Arrange
        var gameState = new GameState();
        // Fill board to create draw
        gameState.TryMakeMove(0, 0); // X
        gameState.TryMakeMove(0, 1); // O
        gameState.TryMakeMove(0, 2); // X
        gameState.TryMakeMove(1, 0); // O
        gameState.TryMakeMove(1, 1); // X
        gameState.TryMakeMove(2, 0); // O
        gameState.TryMakeMove(1, 2); // X
        gameState.TryMakeMove(2, 2); // O
        gameState.TryMakeMove(2, 1); // X - Draw

        // Act
        var result = gameState.TryMakeMove(0, 0); // Try to make another move

        // Assert
        Assert.False(result);
        Assert.Equal(GameStatus.Draw, gameState.Status);
    }

    [Fact]
    public void TryMakeMove_MultipleValidMoves_ShouldAlternatePlayers()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.TryMakeMove(0, 0); // X
        gameState.TryMakeMove(0, 1); // O
        gameState.TryMakeMove(1, 0); // X

        // Assert
        Assert.Equal(Player.O, gameState.CurrentPlayer);
        Assert.Equal(3, gameState.MoveHistory.Count);
        Assert.Equal(Player.X, gameState.MoveHistory[0].Player);
        Assert.Equal(Player.O, gameState.MoveHistory[1].Player);
        Assert.Equal(Player.X, gameState.MoveHistory[2].Player);
    }
}
