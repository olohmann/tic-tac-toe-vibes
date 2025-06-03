/**
 * Tic Tac Toe Game - Simple Web UI
 * 
 * This JavaScript implementation provides the game logic and API integration
 * for a Tic Tac Toe web interface that communicates with the TicTacToe.WebAPI.
 */

// Configuration
const CONFIG = {
    API_BASE_URL: 'http://localhost:5118/api',
    STORAGE_KEY: 'tictactoe-game-id',
    ANIMATION_DURATION: 300,
    ERROR_DISPLAY_TIME: 5000,
    RETRY_ATTEMPTS: 3,
    RETRY_DELAY: 1000
};

// Game status constants
const GAME_STATUS = {
    IN_PROGRESS: 'InProgress',
    X_WINS: 'XWins',
    O_WINS: 'OWins',
    DRAW: 'Draw'
};

// Player constants
const PLAYER = {
    X: 'X',
    O: 'O'
};

/**
 * API Service for communicating with the TicTacToe WebAPI
 */
class GameApiService {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
    }

    /**
     * Create a new game
     * @returns {Promise<Object>} Game state object
     */
    async createGame() {
        const response = await this.makeRequest('POST', '/games');
        return response;
    }

    /**
     * Get current game state
     * @param {string} gameId - The game ID
     * @returns {Promise<Object>} Game state object
     */
    async getGame(gameId) {
        const response = await this.makeRequest('GET', `/games/${gameId}`);
        return response;
    }

    /**
     * Make a move in the game
     * @param {string} gameId - The game ID
     * @param {number} position - Position (0-8) on the board
     * @returns {Promise<Object>} Updated game state
     */
    async makeMove(gameId, position) {
        const response = await this.makeRequest('POST', `/games/${gameId}/moves`, {
            position: position
        });
        return response;
    }

    /**
     * Make HTTP request with error handling and retries
     * @param {string} method - HTTP method
     * @param {string} endpoint - API endpoint
     * @param {Object} body - Request body
     * @returns {Promise<Object>} Response data
     */
    async makeRequest(method, endpoint, body = null) {
        const url = `${this.baseUrl}${endpoint}`;
        const options = {
            method,
            headers: {
                'Content-Type': 'application/json',
            },
        };

        if (body) {
            options.body = JSON.stringify(body);
        }

        let lastError;
        for (let attempt = 1; attempt <= CONFIG.RETRY_ATTEMPTS; attempt++) {
            try {
                const response = await fetch(url, options);
                
                if (!response.ok) {
                    const errorData = await response.json().catch(() => ({}));
                    throw new Error(errorData.message || `HTTP ${response.status}: ${response.statusText}`);
                }

                return await response.json();
            } catch (error) {
                lastError = error;
                console.warn(`API request attempt ${attempt} failed:`, error.message);
                
                if (attempt < CONFIG.RETRY_ATTEMPTS) {
                    await this.delay(CONFIG.RETRY_DELAY * attempt);
                }
            }
        }

        throw lastError;
    }

    /**
     * Delay helper for retries
     * @param {number} ms - Milliseconds to delay
     */
    delay(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
}

/**
 * UI Controller for managing the game interface
 */
class GameUIController {
    constructor(game) {
        this.game = game;
        this.elements = this.initializeElements();
        this.bindEvents();
    }

    /**
     * Initialize DOM element references
     */
    initializeElements() {
        return {
            gameGrid: document.getElementById('game-grid'),
            gameStatus: document.getElementById('game-status'),
            newGameBtn: document.getElementById('new-game-btn'),
            newGameOverlay: document.getElementById('new-game-overlay'),
            startNewGameBtn: document.getElementById('start-new-game-btn'),
            errorMessage: document.getElementById('error-message'),
            errorText: document.getElementById('error-text'),
            closeError: document.getElementById('close-error'),
            apiStatus: document.getElementById('api-status'),
            statusText: document.querySelector('.status-text'),
            cells: Array.from(document.querySelectorAll('.cell'))
        };
    }

    /**
     * Bind event listeners
     */
    bindEvents() {
        // Cell click events
        this.elements.cells.forEach((cell, index) => {
            cell.addEventListener('click', () => this.handleCellClick(index));
            cell.addEventListener('keydown', (e) => {
                if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault();
                    this.handleCellClick(index);
                }
            });
        });

        // Button events
        this.elements.newGameBtn.addEventListener('click', () => this.showNewGameOverlay());
        this.elements.startNewGameBtn.addEventListener('click', () => this.startNewGame());
        this.elements.closeError.addEventListener('click', () => this.hideError());

        // Keyboard navigation
        this.elements.gameGrid.addEventListener('keydown', (e) => this.handleKeyboardNavigation(e));

        // Overlay click to close
        this.elements.newGameOverlay.addEventListener('click', (e) => {
            if (e.target === this.elements.newGameOverlay) {
                this.hideNewGameOverlay();
            }
        });
    }

    /**
     * Handle cell click events
     * @param {number} position - Cell position (0-8)
     */
    async handleCellClick(position) {
        try {
            await this.game.makeMove(position);
        } catch (error) {
            this.showError(error.message);
        }
    }

    /**
     * Handle keyboard navigation in the grid
     * @param {KeyboardEvent} e - Keyboard event
     */
    handleKeyboardNavigation(e) {
        const focusedElement = document.activeElement;
        const currentIndex = this.elements.cells.indexOf(focusedElement);
        
        if (currentIndex === -1) return;

        let newIndex = currentIndex;
        
        switch (e.key) {
            case 'ArrowLeft':
                newIndex = currentIndex % 3 === 0 ? currentIndex + 2 : currentIndex - 1;
                break;
            case 'ArrowRight':
                newIndex = currentIndex % 3 === 2 ? currentIndex - 2 : currentIndex + 1;
                break;
            case 'ArrowUp':
                newIndex = currentIndex < 3 ? currentIndex + 6 : currentIndex - 3;
                break;
            case 'ArrowDown':
                newIndex = currentIndex > 5 ? currentIndex - 6 : currentIndex + 3;
                break;
            default:
                return;
        }

        e.preventDefault();
        this.elements.cells[newIndex].focus();
    }

    /**
     * Update the game board display
     * @param {Object} gameState - Current game state
     */
    updateBoard(gameState) {
        const { board, currentPlayer, status } = gameState;

        // Update cells
        this.elements.cells.forEach((cell, index) => {
            const cellContent = cell.querySelector('.cell-content');
            const boardValue = board[index];
            
            // Clear previous states
            cell.classList.remove('occupied', 'winning');
            cellContent.classList.remove('player-x', 'player-o');
            cellContent.textContent = '';
            
            // Set cell content and styling
            if (boardValue) {
                cellContent.textContent = boardValue;
                cellContent.classList.add(`player-${boardValue.toLowerCase()}`);
                cell.classList.add('occupied');
                cell.disabled = true;
                cell.setAttribute('aria-label', `Cell ${index + 1}, ${boardValue}`);
            } else {
                cell.disabled = status !== GAME_STATUS.IN_PROGRESS;
                cell.setAttribute('aria-label', `Cell ${index + 1}, empty`);
            }
        });

        // Update game status
        this.updateGameStatus(status, currentPlayer);

        // Handle game end
        if (status !== GAME_STATUS.IN_PROGRESS) {
            this.handleGameEnd(status, board);
        }
    }

    /**
     * Update the game status display
     * @param {string} status - Game status
     * @param {string} currentPlayer - Current player (X or O)
     */
    updateGameStatus(status, currentPlayer) {
        let statusText = '';
        
        switch (status) {
            case GAME_STATUS.IN_PROGRESS:
                statusText = `Player ${currentPlayer}'s turn`;
                break;
            case GAME_STATUS.X_WINS:
                statusText = 'Player X Wins! 🎉';
                break;
            case GAME_STATUS.O_WINS:
                statusText = 'Player O Wins! 🎉';
                break;
            case GAME_STATUS.DRAW:
                statusText = "It's a Draw! 🤝";
                break;
            default:
                statusText = 'Unknown game status';
        }

        this.elements.gameStatus.textContent = statusText;
        this.elements.gameStatus.setAttribute('aria-label', statusText);
    }

    /**
     * Handle game end - highlight winning combination if applicable
     * @param {string} status - Game status
     * @param {Array} board - Game board state
     */
    handleGameEnd(status, board) {
        if (status === GAME_STATUS.X_WINS || status === GAME_STATUS.O_WINS) {
            const winningCombination = this.findWinningCombination(board);
            if (winningCombination) {
                winningCombination.forEach(index => {
                    this.elements.cells[index].classList.add('winning');
                });
            }
        }

        // Show new game overlay after a delay
        setTimeout(() => {
            this.showNewGameOverlay();
        }, 2000);
    }

    /**
     * Find the winning combination on the board
     * @param {Array} board - Game board state
     * @returns {Array|null} Winning cell indices or null
     */
    findWinningCombination(board) {
        const winPatterns = [
            [0, 1, 2], [3, 4, 5], [6, 7, 8], // Rows
            [0, 3, 6], [1, 4, 7], [2, 5, 8], // Columns
            [0, 4, 8], [2, 4, 6]             // Diagonals
        ];

        for (const pattern of winPatterns) {
            const [a, b, c] = pattern;
            if (board[a] && board[a] === board[b] && board[a] === board[c]) {
                return pattern;
            }
        }

        return null;
    }

    /**
     * Show error message
     * @param {string} message - Error message to display
     */
    showError(message) {
        this.elements.errorText.textContent = message;
        this.elements.errorMessage.classList.remove('hidden');
        
        // Auto-hide after delay
        setTimeout(() => {
            this.hideError();
        }, CONFIG.ERROR_DISPLAY_TIME);
    }

    /**
     * Hide error message
     */
    hideError() {
        this.elements.errorMessage.classList.add('hidden');
    }

    /**
     * Show new game overlay
     */
    showNewGameOverlay() {
        this.elements.newGameOverlay.classList.remove('hidden');
        this.elements.startNewGameBtn.focus();
    }

    /**
     * Hide new game overlay
     */
    hideNewGameOverlay() {
        this.elements.newGameOverlay.classList.add('hidden');
    }

    /**
     * Start a new game
     */
    async startNewGame() {
        try {
            this.hideNewGameOverlay();
            await this.game.createNewGame();
        } catch (error) {
            this.showError(error.message);
        }
    }

    /**
     * Update API connection status
     * @param {string} status - Connection status: 'connected', 'disconnected', 'connecting'
     */
    updateApiStatus(status) {
        this.elements.apiStatus.className = `api-status ${status}`;
        
        const statusMessages = {
            connected: 'Connected',
            disconnected: 'Disconnected',
            connecting: 'Connecting...'
        };
        
        this.elements.statusText.textContent = statusMessages[status] || 'Unknown';
    }
}

/**
 * Main Tic Tac Toe Game Class
 */
class TicTacToeGame {
    constructor(apiBaseUrl) {
        this.apiService = new GameApiService(apiBaseUrl);
        this.uiController = new GameUIController(this);
        this.currentGameId = null;
        this.currentGameState = null;
    }

    /**
     * Initialize the game application
     */
    async initialize() {
        try {
            this.uiController.updateApiStatus('connecting');
            
            // Try to load existing game or create new one
            const savedGameId = localStorage.getItem(CONFIG.STORAGE_KEY);
            if (savedGameId) {
                try {
                    await this.loadGame(savedGameId);
                } catch (error) {
                    console.warn('Failed to load saved game, creating new one:', error.message);
                    await this.createNewGame();
                }
            } else {
                await this.createNewGame();
            }
            
            this.uiController.updateApiStatus('connected');
        } catch (error) {
            this.uiController.updateApiStatus('disconnected');
            this.uiController.showError('Failed to connect to game server. Please try again.');
            console.error('Game initialization failed:', error);
        }
    }

    /**
     * Create a new game
     */
    async createNewGame() {
        try {
            this.uiController.updateApiStatus('connecting');
            const gameState = await this.apiService.createGame();
            this.currentGameId = gameState.gameId;
            this.currentGameState = gameState;
            
            // Save game ID to localStorage
            localStorage.setItem(CONFIG.STORAGE_KEY, this.currentGameId);
            
            this.uiController.updateBoard(gameState);
            this.uiController.updateApiStatus('connected');
            
            console.log('New game created:', this.currentGameId);
        } catch (error) {
            this.uiController.updateApiStatus('disconnected');
            throw new Error('Failed to create new game. Please check your connection.');
        }
    }

    /**
     * Load an existing game
     * @param {string} gameId - Game ID to load
     */
    async loadGame(gameId) {
        try {
            const gameState = await this.apiService.getGame(gameId);
            this.currentGameId = gameId;
            this.currentGameState = gameState;
            this.uiController.updateBoard(gameState);
            
            console.log('Game loaded:', gameId);
        } catch (error) {
            // Remove invalid game ID from storage
            localStorage.removeItem(CONFIG.STORAGE_KEY);
            throw new Error('Saved game session expired or not found.');
        }
    }

    /**
     * Make a move in the current game
     * @param {number} position - Position (0-8) on the board
     */
    async makeMove(position) {
        if (!this.currentGameId) {
            throw new Error('No active game. Please start a new game.');
        }

        if (this.currentGameState.status !== GAME_STATUS.IN_PROGRESS) {
            throw new Error('Game has already ended. Please start a new game.');
        }

        if (this.currentGameState.board[position]) {
            throw new Error('That position is already taken.');
        }

        try {
            this.uiController.updateApiStatus('connecting');
            const gameState = await this.apiService.makeMove(this.currentGameId, position);
            this.currentGameState = gameState;
            this.uiController.updateBoard(gameState);
            this.uiController.updateApiStatus('connected');
            
            console.log('Move made at position:', position);
        } catch (error) {
            this.uiController.updateApiStatus('disconnected');
            
            // Handle specific API errors
            if (error.message.includes('already taken') || error.message.includes('invalid')) {
                throw new Error('Invalid move. Please try a different position.');
            } else if (error.message.includes('not found')) {
                // Game session expired
                localStorage.removeItem(CONFIG.STORAGE_KEY);
                throw new Error('Game session expired. Please start a new game.');
            } else {
                throw new Error('Failed to make move. Please try again.');
            }
        }
    }

    /**
     * Get current game state
     */
    async getGameState() {
        if (!this.currentGameId) {
            return null;
        }

        try {
            const gameState = await this.apiService.getGame(this.currentGameId);
            this.currentGameState = gameState;
            return gameState;
        } catch (error) {
            console.error('Failed to get game state:', error);
            return null;
        }
    }
}

/**
 * Application entry point
 */
document.addEventListener('DOMContentLoaded', async () => {
    try {
        const game = new TicTacToeGame(CONFIG.API_BASE_URL);
        await game.initialize();
        
        // Make game globally accessible for debugging
        window.ticTacToeGame = game;
        
        console.log('Tic Tac Toe game initialized successfully!');
    } catch (error) {
        console.error('Failed to initialize game:', error);
        
        // Show fallback error message
        const errorDiv = document.createElement('div');
        errorDiv.style.cssText = `
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: #ff4444;
            color: white;
            padding: 20px;
            border-radius: 10px;
            text-align: center;
            z-index: 9999;
            font-family: system-ui, sans-serif;
        `;
        errorDiv.innerHTML = `
            <h3>Failed to Load Game</h3>
            <p>Please check your internet connection and try refreshing the page.</p>
            <button onclick="location.reload()" style="
                background: white;
                color: #ff4444;
                border: none;
                padding: 10px 20px;
                border-radius: 5px;
                cursor: pointer;
                margin-top: 10px;
            ">Retry</button>
        `;
        document.body.appendChild(errorDiv);
    }
});

// Handle page visibility changes
document.addEventListener('visibilitychange', async () => {
    if (!document.hidden && window.ticTacToeGame) {
        // Refresh game state when page becomes visible again
        try {
            await window.ticTacToeGame.getGameState();
        } catch (error) {
            console.warn('Failed to refresh game state:', error);
        }
    }
});

// Handle online/offline events
window.addEventListener('online', () => {
    if (window.ticTacToeGame) {
        window.ticTacToeGame.uiController.updateApiStatus('connected');
    }
});

window.addEventListener('offline', () => {
    if (window.ticTacToeGame) {
        window.ticTacToeGame.uiController.updateApiStatus('disconnected');
    }
});

// Export for testing purposes
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        TicTacToeGame,
        GameApiService,
        GameUIController,
        CONFIG,
        GAME_STATUS,
        PLAYER
    };
}
