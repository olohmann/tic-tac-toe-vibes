using TicTacToe.WebAPI.Services;

namespace TicTacToe.WebAPI.Tests.Services;

/// <summary>
/// Tests for the InMemoryGameRepository class.
/// </summary>
public class InMemoryGameRepositoryTests
{
    [Fact]
    public void CreateGame_ShouldReturnNewGameWithUniqueId()
    {
        // Arrange
        var repository = new InMemoryGameRepository();

        // Act
        var (gameId1, game1) = repository.CreateGame();
        var (gameId2, game2) = repository.CreateGame();

        // Assert
        Assert.NotEqual(Guid.Empty, gameId1);
        Assert.NotEqual(Guid.Empty, gameId2);
        Assert.NotEqual(gameId1, gameId2);
        Assert.NotNull(game1);
        Assert.NotNull(game2);
        Assert.NotSame(game1, game2);
    }

    [Fact]
    public void GetGame_ExistingGame_ShouldReturnGame()
    {
        // Arrange
        var repository = new InMemoryGameRepository();
        var (gameId, originalGame) = repository.CreateGame();

        // Act
        var retrievedGame = repository.GetGame(gameId);

        // Assert
        Assert.NotNull(retrievedGame);
        Assert.Same(originalGame, retrievedGame);
    }

    [Fact]
    public void GetGame_NonExistingGame_ShouldReturnNull()
    {
        // Arrange
        var repository = new InMemoryGameRepository();
        var nonExistentId = Guid.NewGuid();

        // Act
        var game = repository.GetGame(nonExistentId);

        // Assert
        Assert.Null(game);
    }

    [Fact]
    public void UpdateGame_ShouldUpdateGameInRepository()
    {
        // Arrange
        var repository = new InMemoryGameRepository();
        var (gameId, originalGame) = repository.CreateGame();
        var newGame = new TicTacToe.Domain.TicTacToeGame();
        newGame.MakeMove(0, 0); // Make a move to differentiate

        // Act
        repository.UpdateGame(gameId, newGame);
        var retrievedGame = repository.GetGame(gameId);

        // Assert
        Assert.NotNull(retrievedGame);
        Assert.Same(newGame, retrievedGame);
        Assert.NotSame(originalGame, retrievedGame);
    }
}