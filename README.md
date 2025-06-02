# Tic Tac Toe Vibes 🎮

A vibe coding playground for developing a robust tic tac toe game engine and user interface.

This project showcases modern .NET 9 development with clean architecture, comprehensive testing, and multiple presentation layers including CLI and Web API interfaces.

## Projects

### 🎯 Core Domain Logic
- **[TicTacToe.Domain](TicTacToe.Domain/)** - Game engine with complete rule implementation
- **[TicTacToe.Domain.Tests](TicTacToe.Domain.Tests/)** - Comprehensive test suite with 100% branch coverage

### 🖥️ Command Line Interface  
- **[TicTacToe.CLI](TicTacToe.CLI/)** - Interactive console game with colorful output
- **[TicTacToe.CLI.Tests](TicTacToe.CLI.Tests/)** - CLI component testing

### 🌐 Web API
- **[TicTacToe.WebAPI](TicTacToe.WebAPI/)** - REST API for game management
- **[TicTacToe.WebAPI.Tests](TicTacToe.WebAPI.Tests/)** - API integration and unit testing

## Features

- **Complete game logic** following the specification in [`docs/01-spec-core-game.md`](docs/01-spec-core-game.md)
- **100% branch coverage** with comprehensive xUnit tests
- **Modern C# 12** features with nullable reference types enabled
- **Clean Architecture** with separated concerns
- **Multiple interfaces** - CLI and Web API
- **Immutable Move records** for game history tracking
- **Thread-safe read operations** with defensive copying
- **Comprehensive XML documentation** for all public APIs

## Architecture

### Core Classes

- **[`Player`](TicTacToe.Domain/TicTacToeGame.cs)** - Enum representing X and O players
- **[`GameStatus`](TicTacToe.Domain/TicTacToeGame.cs)** - Enum representing game states (InProgress, X_Won, O_Won, Draw)
- **[`Move`](TicTacToe.Domain/TicTacToeGame.cs)** - Immutable record of a player's move with position and metadata
- **[`GameState`](TicTacToe.Domain/TicTacToeGame.cs)** - Core game state management with move validation and win detection
- **[`TicTacToeGame`](TicTacToe.Domain/TicTacToeGame.cs)** - High-level game facade for easy integration

### Game Rules

1. **X always goes first**
2. **Players alternate turns** until game ends
3. **Win conditions**: 3 in a row (horizontal, vertical, or diagonal)
4. **Draw condition**: Board full with no winner
5. **Move validation**: Position must be empty and within bounds

## Quick Start

### Playing via CLI

```bash
cd TicTacToe.CLI
dotnet run
```
