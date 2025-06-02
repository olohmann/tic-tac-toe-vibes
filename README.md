# Tic Tac Toe Game - Domain Logic

A robust implementation of Tic Tac Toe game logic in .NET 9 with comprehensive test coverage.

## Features

- **Complete game logic** following the specification in `docs/spec.md`
- **100% branch coverage** with comprehensive xUnit tests
- **Modern C# 12** features with nullable reference types enabled
- **Immutable Move records** for game history tracking
- **Thread-safe read operations** with defensive copying
- **Comprehensive XML documentation** for all public APIs

## Architecture

### Core Classes

- **`Player`** - Enum representing X and O players
- **`GameStatus`** - Enum representing game states (InProgress, X_Won, O_Won, Draw)
- **`Move`** - Immutable record of a player's move with position and metadata
- **`GameState`** - Core game state management with move validation and win detection
- **`TicTacToeGame`** - High-level game facade for easy integration

### Game Rules

1. **X always goes first**
2. **Players alternate turns** until game ends
3. **Win conditions**: 3 in a row (horizontal, vertical, or diagonal)
4. **Draw condition**: Board full with no winner
5. **Move validation**: Position must be empty and within bounds

## Usage Example

```csharp
using TicTacToe.Domain;

// Create a new game
var game = new TicTacToeGame();

// Make moves
bool success = game.MakeMove(1, 1); // X plays center
success = game.MakeMove(0, 0);      // O plays top-left
success = game.MakeMove(0, 1);      // X plays top-center
success = game.MakeMove(0, 2);      // O plays top-right
success = game.MakeMove(2, 1);      // X plays bottom-center and wins!

// Check game state
var state = game.GetGameState();
Console.WriteLine($"Status: {state.Status}"); // Status: X_Won
Console.WriteLine($"Moves: {state.MoveHistory.Count}"); // Moves: 5

// Get board state
char[,] board = state.GetBoard();
char centerCell = state.GetCell(1, 1); // 'X'

// Start a new game
game.StartNewGame();
```

## Building and Testing

```bash
# Build the solution
dotnet build

# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Format code
dotnet format
```

## Test Coverage

The test suite achieves **100% branch coverage** with 36 comprehensive test cases covering:

- ✅ All winning combinations (rows, columns, diagonals)
- ✅ Draw scenarios
- ✅ Invalid move handling
- ✅ Boundary conditions
- ✅ Game state transitions
- ✅ Move history tracking
- ✅ Player alternation
- ✅ Post-game move prevention

## Project Structure

```
TicTacToe.sln
├── TicTacToe.Domain/           # Core game logic
│   ├── TicTacToeGame.cs       # Main implementation
│   └── TicTacToe.Domain.csproj
├── TicTacToe.Domain.Tests/     # Comprehensive test suite
│   ├── UnitTest1.cs           # All test cases
│   └── TicTacToe.Domain.Tests.csproj
└── docs/
    └── spec.md                 # Game specification
```

## Quality Standards

- **Nullable reference types** enabled for null safety
- **ImplicitUsings** for cleaner code
- **TreatWarningsAsErrors** for zero-warning builds
- **XML documentation** on all public APIs
- **Expression-bodied members** where appropriate
- **Guard clauses** for input validation
- **Microsoft C# coding standards** compliance
