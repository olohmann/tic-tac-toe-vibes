## Tic Tac Toe — Simple Web UI Specification

### Overview
A simple, responsive web interface for playing Tic Tac Toe games using pure HTML, CSS, and JavaScript. The interface communicates with the existing [`TicTacToe.WebAPI`](../TicTacToe.WebAPI/TicTacToe.WebAPI.csproj) via REST API calls.

### Project Structure
- **TicTacToe.SimpleWebUI** - Static web files (HTML, CSS, JS)
- **TicTacToe.SimpleWebUI.Tests** - Optional JavaScript testing with Jest or similar

### File Structure
```
TicTacToe.SimpleWebUI/
├── index.html              # Main game page
├── styles.css              # Game styling
├── script.js               # Game logic and API calls
├── favicon.ico             # Game icon
└── README.md               # Setup instructions
```

### Technical Requirements
- **Pure JavaScript** (ES6+) - No frameworks or libraries
- **Responsive CSS** - Works on desktop and mobile
- **Fetch API** - For REST API communication
- **Local Storage** - Save current game ID for page refresh
- **Error Handling** - User-friendly error messages
- **Accessibility** - Keyboard navigation and screen reader support

### User Interface Components

#### Game Board
- **3x3 Grid** - Interactive cells for moves
- **Visual States** - Empty, X, O with distinct styling
- **Hover Effects** - Preview move before clicking
- **Winning Line** - Highlight winning combination
- **Disabled State** - When game is finished

#### Game Controls
- **New Game Button** - Start fresh game
- **Game Status** - Current player turn or game result
- **API Status** - Connection indicator

#### Layout
Layout will be provided by a FIGMA design.

- **Header** - Game title and controls
- **Main** - Game board (center focus)
- **Footer** - Game information and status
- **Responsive** - Single column on mobile

### API Integration

#### Game Flow
1. **Page Load** - Check localStorage for existing game or create new
2. **Cell Click** - Validate and send move to API
3. **Update UI** - Reflect API response on board
4. **Game End** - Display result and offer new game
5. **Error Handling** - Show user-friendly messages

#### API Calls
```javascript
// Create new game
POST /api/games
Response: { gameId, board, currentPlayer, status }

// Get game state  
GET /api/games/{gameId}
Response: { gameId, board, currentPlayer, status }

// Make move
POST /api/games/{gameId}/moves
Body: { position: 0-8 }
Response: { gameId, board, currentPlayer, status, lastMove }
```

### Styling Guidelines
- **Color Scheme** - Modern, accessible contrast ratios
- **Typography** - Clean, readable fonts (system fonts preferred)
- **Grid Layout** - CSS Grid for game board
- **Animations** - Subtle transitions for moves and state changes
- **Mobile First** - Responsive design approach
- **Dark/Light Mode** - Optional theme toggle

### JavaScript Architecture
```javascript
// Main game class
class TicTacToeGame {
  constructor(apiBaseUrl)
  async createNewGame()
  async makeMove(position)
  async getGameState()
  updateUI(gameState)
  handleError(error)
}

// API service
class GameApiService {
  constructor(baseUrl)
  async createGame()
  async getGame(gameId)
  async makeMove(gameId, position)
}

// UI controller
class GameUIController {
  constructor(game)
  initializeBoard()
  updateBoard(gameState)
  showGameStatus(status, currentPlayer)
  handleCellClick(position)
  showError(message)
}
```

### Error Handling
- **Network Errors** - "Unable to connect to game server"
- **Invalid Moves** - "That position is already taken"
- **Game Not Found** - "Game session expired, starting new game"
- **Server Errors** - "Server error, please try again"

### Accessibility Features
- **Semantic HTML** - Proper heading structure and landmarks
- **ARIA Labels** - Screen reader descriptions for game state
- **Keyboard Navigation** - Tab through cells, Enter/Space to select
- **Focus Management** - Clear focus indicators
- **Color Independence** - Don't rely solely on color for game state

### Browser Support
- **Modern Browsers** - Chrome 80+, Firefox 74+, Safari 13+, Edge 80+
- **ES6+ Features** - Classes, async/await, template literals
- **CSS Grid** - For responsive layout
- **Fetch API** - For HTTP requests

### Configuration
```javascript
// Configuration object
const CONFIG = {
  API_BASE_URL: 'http://localhost:5118/api',
  STORAGE_KEY: 'tictactoe-game-id',
  ANIMATION_DURATION: 300,
  ERROR_DISPLAY_TIME: 5000
};
```

### Testing Strategy
- **Manual Testing** - Cross-browser compatibility

### Development Guidelines
- **ES6+ Syntax** - Use modern JavaScript features
- **CSS Custom Properties** - For theming and consistency
- **Progressive Enhancement** - Basic functionality without JavaScript
- **Performance** - Minimize HTTP requests and DOM manipulation
- **Code Organization** - Separate concerns (API, UI, Game Logic)

### Deployment
- **Static Hosting** - Can be served from any web server
- **CORS Configuration** - Ensure API allows web UI domain
- **Environment Config** - Different API URLs for dev/prod
- **Cache Headers** - Appropriate caching for static assets
