namespace TicTacToe.WebAPI.Exceptions;

/// <summary>
/// Exception thrown when a game is not found.
/// </summary>
public class GameNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the GameNotFoundException class.
    /// </summary>
    /// <param name="gameId">The game ID that was not found.</param>
    public GameNotFoundException(Guid gameId) 
        : base($"Game with ID '{gameId}' not found.")
    {
        GameId = gameId;
    }

    /// <summary>
    /// Gets the game ID that was not found.
    /// </summary>
    public Guid GameId { get; }
}

/// <summary>
/// Exception thrown when an invalid move is attempted.
/// </summary>
public class InvalidMoveException : Exception
{
    /// <summary>
    /// Initializes a new instance of the InvalidMoveException class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public InvalidMoveException(string message) : base(message)
    {
    }
}

/// <summary>
/// Exception thrown when attempting to make a move in a finished game.
/// </summary>
public class GameFinishedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the GameFinishedException class.
    /// </summary>
    public GameFinishedException() : base("Game already finished.")
    {
    }
}