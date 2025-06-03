# Tic Tac Toe - Simple Web UI

A responsive web interface for playing Tic Tac Toe games using pure HTML, CSS, and JavaScript. The interface communicates with the [TicTacToe.WebAPI](../TicTacToe.WebAPI/) via REST API calls.

## Features

- **Pure JavaScript** (ES6+) - No frameworks or libraries required
- **Responsive Design** - Works seamlessly on desktop and mobile devices
- **Accessibility** - Full keyboard navigation and screen reader support
- **Modern UI** - Based on provided Figma design with smooth animations
- **Error Handling** - User-friendly error messages and retry mechanisms
- **Local Storage** - Maintains game state across page refreshes
- **API Integration** - Real-time communication with the game server

## Getting Started

### Prerequisites

- A modern web browser (Chrome 80+, Firefox 74+, Safari 13+, Edge 80+)
- The [TicTacToe.WebAPI](../TicTacToe.WebAPI/) running on `http://localhost:5118`

### Setup

1. **Start the API Server**
   ```bash
   cd ../TicTacToe.WebAPI
   dotnet run --urls "http://localhost:5118"
   ```

2. **Serve the Web UI**
   
   You can serve the files using any static web server. Here are a few options:

   **Option A: Using Python (if installed)**
   ```bash
   # Python 3
   python -m http.server 8080
   
   # Python 2
   python -m SimpleHTTPServer 8080
   ```

   **Option B: Using Node.js (if installed)**
   ```bash
   npx serve . -p 8080
   ```

   **Option C: Using PHP (if installed)**
   ```bash
   php -S localhost:8080
   ```

   **Option D: Using Live Server (VS Code extension)**
   - Install the "Live Server" extension in VS Code
   - Right-click on `index.html` and select "Open with Live Server"

3. **Open in Browser**
   
   Navigate to `http://localhost:8080` (or the port specified by your chosen server)

4. **Test API Connection** (Optional)
   
   Open `http://localhost:8080/test.html` to verify that the API is working correctly

## Configuration

The game configuration can be modified in `script.js`:

```javascript
const CONFIG = {
    API_BASE_URL: 'http://localhost:5118/api',  // API server URL
    STORAGE_KEY: 'tictactoe-game-id',           // LocalStorage key
    ANIMATION_DURATION: 300,                    // Animation timing (ms)
    ERROR_DISPLAY_TIME: 5000,                  // Error message duration (ms)
    RETRY_ATTEMPTS: 3,                         // API retry attempts
    RETRY_DELAY: 1000                          // Delay between retries (ms)
};
```

## How to Play

1. **Start a New Game**: Click the "New Game" button or refresh the page
2. **Make Moves**: Click on any empty cell to place your mark (X or O)
3. **Win Conditions**: Get three of your marks in a row, column, or diagonal
4. **Game End**: The game automatically detects wins and draws
5. **Play Again**: Click "New Game" when the game ends

## Keyboard Navigation

The game is fully accessible via keyboard:

- **Tab**: Navigate through interactive elements
- **Arrow Keys**: Move between game board cells
- **Enter/Space**: Select a cell or activate a button
- **Escape**: Close overlays (planned feature)

## Browser Support

- **Chrome**: 80+
- **Firefox**: 74+
- **Safari**: 13+
- **Edge**: 80+

## File Structure

```
TicTacToe.SimpleWebUI/
├── index.html              # Main game page
├── styles.css              # Game styling (based on Figma design)
├── script.js               # Game logic and API integration
├── test.html               # API connection test page
└── README.md               # This file
```

## API Integration

The web UI communicates with the TicTacToe.WebAPI using these endpoints:

### Create New Game
```http
POST /api/games
Response: { gameId, board, currentPlayer, status }
```

### Get Game State
```http
GET /api/games/{gameId}
Response: { gameId, board, currentPlayer, status }
```

### Make Move
```http
POST /api/games/{gameId}/moves
Body: { position: 0-8 }
Response: { gameId, board, currentPlayer, status, lastMove }
```

## Error Handling

The application handles various error scenarios:

- **Network Errors**: "Unable to connect to game server"
- **Invalid Moves**: "That position is already taken"
- **Expired Sessions**: "Game session expired, starting new game"
- **Server Errors**: "Server error, please try again"

## Design

The UI design is based on the provided Figma specification with:

- **Color Scheme**: Purple gradient background with contrasting game board
- **Typography**: Fredoka One for headings, Inter for body text
- **Layout**: Centered responsive design that works on all screen sizes
- **Animations**: Smooth transitions and hover effects
- **Accessibility**: High contrast ratios and semantic HTML

## Development

For local development:

1. Make changes to the HTML, CSS, or JavaScript files
2. Refresh your browser to see changes
3. Use browser developer tools for debugging
4. Check the console for any JavaScript errors

### Debugging

The game object is available globally as `window.ticTacToeGame` for debugging:

```javascript
// Check current game state
console.log(window.ticTacToeGame.currentGameState);

// Get game ID
console.log(window.ticTacToeGame.currentGameId);

// Access API service directly
window.ticTacToeGame.apiService.getGame(gameId);
```

## Troubleshooting

### Common Issues

1. **"Failed to connect to game server"**
   - Ensure the WebAPI is running on `http://localhost:5118`
   - Check that CORS is properly configured in the API
   - Verify your internet connection

2. **"Game session expired"**
   - This happens when the server restarts or the game ID becomes invalid
   - Click "New Game" to start fresh

3. **UI not updating**
   - Check browser console for JavaScript errors
   - Ensure all files are properly served (no 404 errors)
   - Try a hard refresh (Ctrl+F5 or Cmd+Shift+R)

4. **Responsive layout issues**
   - Clear browser cache
   - Check that CSS is loading properly
   - Test in different browsers

### Performance

The application is optimized for performance:

- Minimal HTTP requests
- Efficient DOM manipulation
- CSS animations using transforms
- Local storage for persistence
- Progressive enhancement

## Contributing

When making changes:

1. Follow the existing code style and structure
2. Test across different browsers and devices
3. Ensure accessibility standards are maintained
4. Update this README if adding new features

## License

This project is part of the Tic Tac Toe Vibes application suite.
