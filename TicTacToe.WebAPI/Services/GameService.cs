using TicTacToe.Domain;
using TicTacToe.WebAPI.Exceptions;
using TicTacToe.WebAPI.Models;

namespace TicTacToe.WebAPI.Services;

/// <summary>
/// Interface for the game service.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <returns>The response for the newly created game.</returns>
    GameResponseDto CreateGame();

    /// <summary>
    /// Gets the current state of a game.
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    /// <returns>The current game state.</returns>
    /// <exception cref="GameNotFoundException">Thrown when the game is not found.</exception>
    GameResponseDto GetGame(Guid gameId);

    /// <summary>
    /// Makes a move in the game.
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    /// <param name="position">The position to make the move (0-8).</param>
    /// <returns>The response after making the move.</returns>
    /// <exception cref="GameNotFoundException">Thrown when the game is not found.</exception>
    /// <exception cref="InvalidMoveException">Thrown when the move is invalid.</exception>
    /// <exception cref="GameFinishedException">Thrown when the game is already finished.</exception>
    MoveGameResponseDto MakeMove(Guid gameId, int position);
}

/// <summary>
/// Implementation of the game service.
/// </summary>
public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;

    /// <summary>
    /// Initializes a new instance of the GameService class.
    /// </summary>
    /// <param name="gameRepository">The game repository.</param>
    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    /// <inheritdoc />
    public GameResponseDto CreateGame()
    {
        var (gameId, game) = _gameRepository.CreateGame();
        return MappingService.ToGameResponseDto(gameId, game.GetGameState());
    }

    /// <inheritdoc />
    public GameResponseDto GetGame(Guid gameId)
    {
        var game = _gameRepository.GetGame(gameId);
        if (game == null)
        {
            throw new GameNotFoundException(gameId);
        }

        return MappingService.ToGameResponseDto(gameId, game.GetGameState());
    }

    /// <inheritdoc />
    public MoveGameResponseDto MakeMove(Guid gameId, int position)
    {
        var game = _gameRepository.GetGame(gameId);
        if (game == null)
        {
            throw new GameNotFoundException(gameId);
        }

        var gameState = game.GetGameState();
        
        // Check if game is already finished
        if (gameState.Status != GameStatus.InProgress)
        {
            throw new GameFinishedException();
        }

        // Validate position range
        if (position < 0 || position > 8)
        {
            throw new InvalidMoveException("Position must be between 0 and 8.");
        }

        // Convert position to coordinates
        var (row, col) = MappingService.PositionToCoordinates(position);

        // Check if position is already occupied
        if (gameState.GetCell(row, col) != ' ')
        {
            throw new InvalidMoveException("Invalid move: position already occupied");
        }

        // Make the move
        var success = game.MakeMove(row, col);
        if (!success)
        {
            throw new InvalidMoveException("Invalid move: position already occupied");
        }

        // Get the updated game state and last move
        var updatedGameState = game.GetGameState();
        var lastMove = updatedGameState.MoveHistory.Last();

        // Update repository (though not strictly necessary for in-memory)
        _gameRepository.UpdateGame(gameId, game);

        return MappingService.ToMoveGameResponseDto(gameId, updatedGameState, lastMove);
    }
}