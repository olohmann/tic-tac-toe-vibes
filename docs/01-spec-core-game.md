## Tic Tac Toe — Core Game Specification (Condensed)

### 1. Objective

Two players compete on a 3 × 3 grid. The first to align three identical marks (**X** or **O**) horizontally, vertically, or diagonally wins. If all nine cells fill without a winning line, the game ends in a draw.

---

### 2. Board Terminology

| Term      | Definition                                                                           |
| --------- | ------------------------------------------------------------------------------------ |
| **Cell**  | One of nine positions, identified by `(row, column)` using zero-based indices `0–2`. |
| **Board** | 3 × 3 matrix holding `' '`, `'X'`, or `'O'`.                                         |

---

### 3. Turn Sequence

1. **Initial state**: Empty board; **X** plays first.
2. **Move**: Current player selects any empty cell and places their mark.
3. **Validation**:
   - If chosen cell is occupied → **Illegal Move** (reject; state unchanged).
4. **Outcome check** (immediately after a legal move):
   - **Win** if current player owns all three cells of any winning line.
   - **Draw** if board is full and no win detected.
5. **Turn hand-off**: If no win or draw, switch `currentPlayer` and continue.

---

### 4. Game State Model

```
GameState
├─ board          : char[3][3]   // ’ ’, ‘X’, ‘O’
├─ currentPlayer  : enum { X, O }
├─ status         : enum { InProgress, X_Won, O_Won, Draw }
├─ moveHistory[]  : Move         // chronological list

Move
├─ row            : 0..2
├─ col            : 0..2
├─ player         : X | O
├─ sequenceNumber : int          // 1-based
```

---

### 5. Functional Requirements

- **Start Game**

  - Initialize `GameState` with empty board, `currentPlayer = X`, `status = InProgress`.

- **Make Move** `(row, col)`

  - Accept only if `status == InProgress`, target cell is empty, and caller matches `currentPlayer`.
  - Update board, append `Move` to history, evaluate outcome, update `status`.

- **Retrieve State**

  - Return full `GameState` snapshot on request (read-only).

- **Restart / New Game**
  - Create a fresh game state independent of prior sessions.

---
