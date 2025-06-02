using TicTacToe.Domain;

namespace TicTacToe.WebAPI.Services;

/// <summary>
/// Interface for the game repository.
/// </summary>
public interface IGameRepository
{
    /// <summary>
    /// Creates a new game and returns its unique identifier.
    /// </summary>
    /// <returns>The game ID and initial game state.</returns>
    (Guid gameId, TicTacToeGame game) CreateGame();

    /// <summary>
    /// Gets a game by its ID.
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    /// <returns>The game if found, null otherwise.</returns>
    TicTacToeGame? GetGame(Guid gameId);

    /// <summary>
    /// Updates a game in the repository.
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    /// <param name="game">The updated game.</param>
    void UpdateGame(Guid gameId, TicTacToeGame game);
}

/// <summary>
/// In-memory implementation of the game repository.
/// </summary>
public class InMemoryGameRepository : IGameRepository
{
    private readonly Dictionary<Guid, TicTacToeGame> _games = [];

    /// <inheritdoc />
    public (Guid gameId, TicTacToeGame game) CreateGame()
    {
        var gameId = Guid.NewGuid();
        var game = new TicTacToeGame();
        _games[gameId] = game;
        return (gameId, game);
    }

    /// <inheritdoc />
    public TicTacToeGame? GetGame(Guid gameId)
    {
        return _games.GetValueOrDefault(gameId);
    }

    /// <inheritdoc />
    public void UpdateGame(Guid gameId, TicTacToeGame game)
    {
        _games[gameId] = game;
    }
}