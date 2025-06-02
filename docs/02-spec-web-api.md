## Tic Tac Toe — Web API Specification

### Overview
REST API for playing Tic Tac Toe games using ASP.NET Core with the existing [`TicTacToe.Domain`](../TicTacToe.Domain/TicTacToe.Domain.csproj) logic.

### Project Structure
- **TicTacToe.WebAPI** - ASP.NET Core Web API project
- **TicTacToe.WebAPI.Tests** - xUnit test project with 100% coverage

### Endpoints

#### POST /api/games
Create a new game.
```json
Response: {
  "gameId": "guid",
  "board": ["", "", "", "", "", "", "", "", ""],
  "currentPlayer": "X",
  "status": "InProgress"
}
```

#### GET /api/games/{gameId}
Get current game state.
```json
Response: {
  "gameId": "guid",
  "board": ["X", "", "O", "", "X", "", "", "", ""],
  "currentPlayer": "O",
  "status": "InProgress|XWins|OWins|Draw"
}
```

#### POST /api/games/{gameId}/moves
Make a move.
```json
Request: { "position": 4 }
Response: {
  "gameId": "guid",
  "board": ["X", "", "O", "", "X", "", "", "", ""],
  "currentPlayer": "O", 
  "status": "InProgress",
  "lastMove": { "player": "X", "position": 4 }
}
```

### Technical Requirements
- **ASP.NET Core 9.0** with minimal APIs or controllers
- **In-memory storage** (Dictionary<Guid, GameState>)
- **Dependency injection** for game services
- **Global exception handling** middleware
- **Input validation** with proper error responses
- **Swagger/OpenAPI** documentation
- **CORS** enabled for development

### Error Responses
```json
400 Bad Request: { "error": "Invalid move: position already occupied" }
404 Not Found: { "error": "Game not found" }
422 Unprocessable Entity: { "error": "Game already finished" }
```

### Testing Strategy
- **Unit tests** for controllers/minimal API handlers
- **Integration tests** for full HTTP request/response cycles using ASP.NET Core's `WebApplicationFactory<T>` with `Microsoft.AspNetCore.Mvc.Testing` for in-memory test server hosting
- **Repository pattern** tests for game storage
- **Validation tests** for all error scenarios

### Implementation Notes
- Reuse [`TicTacToeGame`](../TicTacToe.Domain/TicTacToeGame.cs) from domain layer
- Map domain exceptions to appropriate HTTP status codes
- Use record types for DTOs following C# 12 conventions
- Apply [`TreatWarningsAsErrors`](../.github/copilot-instructions.md) configuration

