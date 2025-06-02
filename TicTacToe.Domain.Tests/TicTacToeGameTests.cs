using TicTacToe.Domain;

namespace TicTacToe.Domain.Tests;

/// <summary>
/// Tests for the TicTacToeGame class.
/// </summary>
public class TicTacToeGameTests
{
    [Fact]
    public void TicTacToeGame_Constructor_ShouldInitializeGameState()
    {
        // Act
        var game = new TicTacToeGame();

        // Assert
        Assert.NotNull(game.GameState);
        Assert.Equal(Player.X, game.GameState.CurrentPlayer);
        Assert.Equal(GameStatus.InProgress, game.GameState.Status);
    }

    [Fact]
    public void StartNewGame_ShouldResetGameState()
    {
        // Arrange
        var game = new TicTacToeGame();
        game.MakeMove(1, 1);

        // Act
        game.StartNewGame();

        // Assert
        Assert.Equal(Player.X, game.GameState.CurrentPlayer);
        Assert.Equal(GameStatus.InProgress, game.GameState.Status);
        Assert.Empty(game.GameState.MoveHistory);
        Assert.Equal(' ', game.GameState.GetCell(1, 1));
    }

    [Fact]
    public void MakeMove_ValidMove_ShouldReturnTrue()
    {
        // Arrange
        var game = new TicTacToeGame();

        // Act
        var result = game.MakeMove(1, 1);

        // Assert
        Assert.True(result);
        Assert.Equal('X', game.GameState.GetCell(1, 1));
    }

    [Fact]
    public void MakeMove_InvalidMove_ShouldReturnFalse()
    {
        // Arrange
        var game = new TicTacToeGame();
        game.MakeMove(1, 1);

        // Act
        var result = game.MakeMove(1, 1); // Same position

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetGameState_ShouldReturnCurrentGameState()
    {
        // Arrange
        var game = new TicTacToeGame();
        game.MakeMove(0, 0);

        // Act
        var gameState = game.GetGameState();

        // Assert
        Assert.Equal(game.GameState, gameState);
        Assert.Equal('X', gameState.GetCell(0, 0));
    }

    [Fact]
    public void CompleteGame_XWins_ShouldWorkCorrectly()
    {
        // Arrange
        var game = new TicTacToeGame();

        // Act
        Assert.True(game.MakeMove(0, 0)); // X
        Assert.True(game.MakeMove(1, 0)); // O
        Assert.True(game.MakeMove(0, 1)); // X
        Assert.True(game.MakeMove(1, 1)); // O
        Assert.True(game.MakeMove(0, 2)); // X wins

        // Assert
        Assert.Equal(GameStatus.X_Won, game.GameState.Status);
        Assert.False(game.MakeMove(2, 2)); // Can't move after game ends
    }

    [Fact]
    public void CompleteGame_Draw_ShouldWorkCorrectly()
    {
        // Arrange
        var game = new TicTacToeGame();

        // Act - Create draw scenario
        game.MakeMove(0, 0); // X
        game.MakeMove(0, 1); // O
        game.MakeMove(0, 2); // X
        game.MakeMove(1, 0); // O
        game.MakeMove(1, 1); // X
        game.MakeMove(2, 0); // O
        game.MakeMove(1, 2); // X
        game.MakeMove(2, 2); // O
        game.MakeMove(2, 1); // X

        // Assert
        Assert.Equal(GameStatus.Draw, game.GameState.Status);
        Assert.Equal(9, game.GameState.MoveHistory.Count);
    }
}
