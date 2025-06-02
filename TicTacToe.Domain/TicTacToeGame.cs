namespace TicTacToe.Domain;

/// <summary>
/// Represents a player in the Tic Tac Toe game.
/// </summary>
public enum Player
{
    /// <summary>
    /// The X player, who moves first.
    /// </summary>
    X,
    
    /// <summary>
    /// The O player, who moves second.
    /// </summary>
    O
}

/// <summary>
/// Represents the current status of the game.
/// </summary>
public enum GameStatus
{
    /// <summary>
    /// The game is still in progress.
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Player X has won the game.
    /// </summary>
    X_Won,
    
    /// <summary>
    /// Player O has won the game.
    /// </summary>
    O_Won,
    
    /// <summary>
    /// The game ended in a draw.
    /// </summary>
    Draw
}

/// <summary>
/// Represents a single move in the game.
/// </summary>
public record Move
{
    /// <summary>
    /// Gets the row position (0-2).
    /// </summary>
    public int Row { get; init; }
    
    /// <summary>
    /// Gets the column position (0-2).
    /// </summary>
    public int Col { get; init; }
    
    /// <summary>
    /// Gets the player who made this move.
    /// </summary>
    public Player Player { get; init; }
    
    /// <summary>
    /// Gets the sequence number of this move (1-based).
    /// </summary>
    public int SequenceNumber { get; init; }
    
    /// <summary>
    /// Initializes a new instance of the Move record.
    /// </summary>
    /// <param name="row">The row position (0-2).</param>
    /// <param name="col">The column position (0-2).</param>
    /// <param name="player">The player making the move.</param>
    /// <param name="sequenceNumber">The sequence number (1-based).</param>
    public Move(int row, int col, Player player, int sequenceNumber)
    {
        Row = row;
        Col = col;
        Player = player;
        SequenceNumber = sequenceNumber;
    }
}

/// <summary>
/// Represents the complete state of a Tic Tac Toe game.
/// </summary>
public class GameState
{
    private readonly char[,] _board = new char[3, 3];
    private readonly List<Move> _moveHistory = [];

    /// <summary>
    /// Gets the current player whose turn it is.
    /// </summary>
    public Player CurrentPlayer { get; private set; } = Player.X;
    
    /// <summary>
    /// Gets the current status of the game.
    /// </summary>
    public GameStatus Status { get; private set; } = GameStatus.InProgress;
    
    /// <summary>
    /// Gets a read-only list of all moves made in chronological order.
    /// </summary>
    public IReadOnlyList<Move> MoveHistory => _moveHistory.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the GameState class with an empty board.
    /// </summary>
    public GameState()
    {
        // Initialize board with empty spaces
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                _board[row, col] = ' ';
            }
        }
    }

    /// <summary>
    /// Gets the character at the specified board position.
    /// </summary>
    /// <param name="row">The row position (0-2).</param>
    /// <param name="col">The column position (0-2).</param>
    /// <returns>The character at the position (' ', 'X', or 'O').</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when row or col is not between 0 and 2.</exception>
    public char GetCell(int row, int col)
    {
        if (row < 0 || row > 2)
            throw new ArgumentOutOfRangeException(nameof(row), "Row must be between 0 and 2.");
        if (col < 0 || col > 2)
            throw new ArgumentOutOfRangeException(nameof(col), "Column must be between 0 and 2.");
            
        return _board[row, col];
    }

    /// <summary>
    /// Gets a copy of the entire board state.
    /// </summary>
    /// <returns>A 3x3 array representing the current board state.</returns>
    public char[,] GetBoard()
    {
        var boardCopy = new char[3, 3];
        Array.Copy(_board, boardCopy, _board.Length);
        return boardCopy;
    }

    /// <summary>
    /// Attempts to make a move at the specified position.
    /// </summary>
    /// <param name="row">The row position (0-2).</param>
    /// <param name="col">The column position (0-2).</param>
    /// <returns>True if the move was successful, false if it was illegal.</returns>
    public bool TryMakeMove(int row, int col)
    {
        // Validate game state
        if (Status != GameStatus.InProgress)
            return false;

        // Validate position bounds
        if (row < 0 || row > 2 || col < 0 || col > 2)
            return false;

        // Validate cell is empty
        if (_board[row, col] != ' ')
            return false;

        // Make the move
        var playerChar = CurrentPlayer == Player.X ? 'X' : 'O';
        _board[row, col] = playerChar;
        
        // Record the move
        var move = new Move(row, col, CurrentPlayer, _moveHistory.Count + 1);
        _moveHistory.Add(move);

        // Check for win or draw
        UpdateGameStatus();

        // Switch players if game continues
        if (Status == GameStatus.InProgress)
        {
            CurrentPlayer = CurrentPlayer == Player.X ? Player.O : Player.X;
        }

        return true;
    }

    private void UpdateGameStatus()
    {
        var currentPlayerChar = CurrentPlayer == Player.X ? 'X' : 'O';
        
        // Check for win
        if (HasWinningLine(currentPlayerChar))
        {
            Status = CurrentPlayer == Player.X ? GameStatus.X_Won : GameStatus.O_Won;
            return;
        }

        // Check for draw (board full)
        if (IsBoardFull())
        {
            Status = GameStatus.Draw;
        }
    }

    private bool HasWinningLine(char playerChar)
    {
        // Check rows
        for (int row = 0; row < 3; row++)
        {
            if (_board[row, 0] == playerChar && 
                _board[row, 1] == playerChar && 
                _board[row, 2] == playerChar)
            {
                return true;
            }
        }

        // Check columns
        for (int col = 0; col < 3; col++)
        {
            if (_board[0, col] == playerChar && 
                _board[1, col] == playerChar && 
                _board[2, col] == playerChar)
            {
                return true;
            }
        }

        // Check diagonals
        if (_board[0, 0] == playerChar && 
            _board[1, 1] == playerChar && 
            _board[2, 2] == playerChar)
        {
            return true;
        }

        if (_board[0, 2] == playerChar && 
            _board[1, 1] == playerChar && 
            _board[2, 0] == playerChar)
        {
            return true;
        }

        return false;
    }

    private bool IsBoardFull()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (_board[row, col] == ' ')
                {
                    return false;
                }
            }
        }
        return true;
    }
}

/// <summary>
/// Core game engine for Tic Tac Toe.
/// </summary>
public class TicTacToeGame
{
    private GameState _gameState;

    /// <summary>
    /// Gets the current game state.
    /// </summary>
    public GameState GameState => _gameState;

    /// <summary>
    /// Initializes a new instance of the TicTacToeGame class.
    /// </summary>
    public TicTacToeGame()
    {
        _gameState = new GameState();
    }

    /// <summary>
    /// Starts a new game, resetting all state.
    /// </summary>
    public void StartNewGame()
    {
        _gameState = new GameState();
    }

    /// <summary>
    /// Makes a move at the specified position for the current player.
    /// </summary>
    /// <param name="row">The row position (0-2).</param>
    /// <param name="col">The column position (0-2).</param>
    /// <returns>True if the move was successful, false if it was illegal.</returns>
    public bool MakeMove(int row, int col) => _gameState.TryMakeMove(row, col);

    /// <summary>
    /// Gets the current game state as a read-only snapshot.
    /// </summary>
    /// <returns>The current GameState instance.</returns>
    public GameState GetGameState() => _gameState;
}
