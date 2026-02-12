# Memory Grid - Script Reference Documentation

Complete documentation of all C# scripts in the Memory Grid game.

## Table of Contents
1. [Tile.cs](#tilecs)
2. [GridManager.cs](#gridmanagercs)
3. [GameManager.cs](#gamemanagercs)
4. [UIManager.cs](#uimanagercs)
5. [SceneGenerator.cs](#scenegeneratorcs)

---

## Tile.cs

**Location**: `Assets/Scripts/Tile.cs`  
**Purpose**: Manages individual tile behavior, visuals, and click detection  
**Dependencies**: None (except UnityEngine)

### Public Methods

#### `void Initialize(int x, int y, GridManager manager)`
Initializes the tile with grid position and game reference.

**Parameters**:
- `x` (int): Column position in grid (0 to gridWidth-1)
- `y` (int): Row position in grid (0 to gridHeight-1)
- `manager` (GridManager): Reference to the grid manager

**Called By**: GridManager during grid creation

**Example**:
```csharp
Tile tile = tileObj.AddComponent<Tile>();
tile.Initialize(0, 0, gridManager);
```

---

#### `void Highlight()`
Turn the tile on by changing its color to bright cyan.

**When Used**:
- When showing the pattern to the player
- When player clicks a correct tile

**Example**:
```csharp
tileGrid[0, 0].Highlight();
```

---

#### `void Unhighlight()`
Turn the tile off by returning it to gray color.

**When Used**:
- After a tile in the pattern finishes displaying
- To reset tile state

**Example**:
```csharp
tileGrid[0, 0].Unhighlight();
```

---

#### `void ResetVisuals()`
Reset tile to initial gray state. Called during initialization.

**Example**:
```csharp
tile.ResetVisuals();
```

---

#### `void FlashError()`
Flash the tile red to indicate a wrong click.

**Behavior**:
- Changes to red for 0.3 seconds
- Returns to normal color
- Uses coroutine for animation

**Example**:
```csharp
tileGrid[0, 0].FlashError();
```

---

#### `bool IsHighlighted()`
Check if the tile is currently highlighted.

**Returns**:
- `true` if tile is bright cyan
- `false` if tile is gray

**Example**:
```csharp
if (tile.IsHighlighted()) {
    // Tile is lit up
}
```

---

### Private Methods

#### `private IEnumerator ErrorFlashCoroutine()`
Coroutine for red flash animation. Automatically called by FlashError().

**Flow**:
1. Set tile color to red
2. Wait 0.3 seconds
3. Restore original color

---

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `gridX` | int | Column position in grid |
| `gridY` | int | Row position in grid |
| `tileImage` | Image | UI Image component for rendering |
| `tileButton` | Button | Button component for clicks |
| `originalColor` | Color | Tile color when not highlighted (gray) |
| `highlightedColor` | Color | Tile color when highlighted (cyan) |
| `isHighlighted` | bool | Is tile currently lit? |
| `gridManager` | GridManager | Reference to grid manager |

---

### Constants

```csharp
private static readonly Color NORMAL_COLOR = new Color(0.7f, 0.7f, 0.7f);
  // Gray: (178, 178, 178) in RGB

private static readonly Color HIGHLIGHT_COLOR = new Color(0.2f, 0.8f, 1f);
  // Cyan: (51, 204, 255) in RGB
```

---

## GridManager.cs

**Location**: `Assets/Scripts/GridManager.cs`  
**Purpose**: Creates and manages the game grid, patterns, and pattern validation  
**Size**: ~400 lines of code with extensive comments

### Public Methods

#### `void Initialize(GameManager manager)`
Initialize the grid manager with game manager reference.

**Parameters**:
- `manager` (GameManager): Reference to the game manager

**Called By**: GameManager.Start()

**Example**:
```csharp
gridManager.Initialize(gameManager);
```

---

#### `void CreateGrid()`
Create the visual game grid by instantiating all tiles.

**Process**:
1. Create grid container GameObject
2. Create tiles at each grid position
3. Add UI components to each tile
4. Center grid on screen
5. Initialize all tiles

**Called By**: GameManager.StartLevel()

**Example**:
```csharp
gridManager.CreateGrid(); // Creates 4x4 = 16 tiles by default
```

---

#### `void AddRandomPatternElement()`
Generate a random tile position and add it to the current pattern.

**Behavior**:
- Picks random X (0 to gridWidth-1)
- Picks random Y (0 to gridHeight-1)
- Adds Vector2Int to currentPattern list

**Called By**: GameManager.RunRound()

**Example**:
```csharp
gridManager.AddRandomPatternElement();
// Pattern now has 1 more element
```

---

#### `IEnumerator ShowPattern()`
Display the current pattern to the player with animation.

**Process**:
1. Disable player clicking
2. Wait 0.5 seconds
3. For each tile in pattern:
   - Highlight tile
   - Wait 1.5 seconds (patternDisplayTime)
   - Unhighlight tile
   - Wait 0.3 seconds between tiles
4. Enable player clicking

**Called By**: GameManager.RunRound()

**Example**:
```csharp
yield return StartCoroutine(gridManager.ShowPattern());
```

---

#### `void ResetPlayerPattern()`
Clear the player's entered pattern and unhighlight all tiles.

**Used For**:
- Resetting after wrong click
- Starting new round

**Example**:
```csharp
gridManager.ResetPlayerPattern();
```

---

#### `int GetPatternLength()`
Get the number of elements in the current pattern (difficulty level).

**Returns**: Number of tiles in currentPattern

**Example**:
```csharp
int difficulty = gridManager.GetPatternLength(); // 1-10+
```

---

#### `void OnTileClicked(int x, int y)`
Handle player tile click. Validates against current pattern.

**Process**:
1. Check if player can click (canPlayerClick && !isShowingPattern)
2. Add click to playerPattern
3. Verify click matches expected pattern position
4. If wrong: call gameManager.OnWrongClick()
5. If right and complete: call gameManager.OnPatternComplete()

**Called By**: Tile.OnTileClicked()

**Example**: Called automatically when player clicks

---

#### `void ClearGrid()`
Destroy all tiles and clean up grid array.

**Used For**:
- Changing grid size
- Scene cleanup

**Example**:
```csharp
gridManager.ClearGrid();
```

---

#### `Vector2Int GetGridSize()`
Get the current grid dimensions.

**Returns**: Vector2Int with (width, height)

**Example**:
```csharp
Vector2Int size = gridManager.GetGridSize(); // (4, 4)
```

---

#### `bool CanPlayerClick()`
Check if player is allowed to click tiles.

**Returns**:
- `true` if pattern is displayed and player can interact
- `false` if showing pattern or waiting

**Example**:
```csharp
if (gridManager.CanPlayerClick()) {
    // Safe to accept clicks
}
```

---

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `gridWidth` | int | Horizontal grid size (4-6) |
| `gridHeight` | int | Vertical grid size (4-6) |
| `tileSize` | float | Pixel size of each tile (100) |
| `spacing` | float | Pixels between tiles (5) |
| `patternDisplayTime` | float | Seconds each tile shows (1.5) |
| `tileGrid` | Tile[,] | 2D array of all tiles |
| `currentPattern` | List<Vector2Int> | Pattern to recreate |
| `playerPattern` | List<Vector2Int> | Player's current input |
| `canPlayerClick` | bool | Can player click now? |
| `isShowingPattern` | bool | Is pattern animating? |

---

## GameManager.cs

**Location**: `Assets/Scripts/GameManager.cs`  
**Purpose**: Main game controller managing levels, lives, and game flow  
**Size**: ~300 lines of code

### Public Methods

#### `void OnPatternComplete()`
Called when player successfully recreates the pattern.

**Behavior**:
1. Show success message
2. Schedule next round after delay
3. Start new round with longer pattern

**Called By**: GridManager.OnTileClicked()

**Example**: Called automatically

---

#### `void OnWrongClick()`
Called when player clicks wrong tile.

**Behavior**:
1. Decrement currentLives
2. Update UI display
3. Check if game over
4. If not: restart level
5. If yes: end game

**Called By**: GridManager.OnTileClicked()

**Example**: Called automatically

---

#### `void RestartGame()`
Reset game to initial state and start over.

**Resets**:
- Level = 1
- Lives = maxLives
- gameOver = false

**Called By**:
- UIManager restart button
- Game over logic

**Example**:
```csharp
gameManager.RestartGame();
```

---

#### `int GetCurrentLevel()`
Get the current level number.

**Returns**: 1-based level number

**Example**:
```csharp
int level = gameManager.GetCurrentLevel(); // 1, 2, 3, ...
```

---

#### `int GetCurrentLives()`
Get remaining lives.

**Returns**: Current lives (0-3)

**Example**:
```csharp
if (gameManager.GetCurrentLives() <= 1) {
    // Almost dead!
}
```

---

#### `bool IsGameActive()`
Check if game is currently running and not over.

**Returns**: true if level is active and not game over

**Example**:
```csharp
if (gameManager.IsGameActive()) {
    // Game is running normally
}
```

---

### Private Methods

#### `private void StartLevel()`
Initialize a new level with fresh grid.

---

#### `private IEnumerator RunRound()`
Main game loop: add pattern, show it, wait for player.

---

#### `private IEnumerator NextRoundAfterDelay()`
Delay before starting next round.

---

#### `private IEnumerator RestartLevelAfterDelay()`
Delay before restarting current level.

---

#### `private void EndGame()`
Handle game over when lives reach 0.

---

#### `private void NextLevel()`
Progress to next level (increases difficulty).

---

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `initialGridWidth` | int | Starting width (4) |
| `initialGridHeight` | int | Starting height (4) |
| `maxLives` | int | Lives per level (3) |
| `delayBetweenRounds` | float | Delay in seconds (2.0) |
| `currentLevel` | int | Current level (1+) |
| `currentLives` | int | Lives remaining (0-3) |
| `isLevelActive` | bool | Is level running? |
| `gameOver` | bool | Has game ended? |

---

### Game Flow Chart

```
StartLevel()
  ↓
CreateGrid()
  ↓
RunRound() [Coroutine]
  ↓
AddRandomPatternElement()
  ↓
ShowPattern() [Coroutine]
  ↓
Wait for player
  ↓
OnTileClicked() from Tile
  ↓
  Wrong Click?  → OnWrongClick() → Lives-1 → Game Over? → EndGame()
    ↓ Yes                                      ↓ No
                                        RestartLevelAfterDelay()
  ↓ No (Correct)
Pattern Complete?
  ↓ No
  Keep playing (click next tile)
  ↓ Yes
OnPatternComplete() → NextRoundAfterDelay() → RunRound() (Loop again)
```

---

## UIManager.cs

**Location**: `Assets/Scripts/UIManager.cs`  
**Purpose**: Create and manage all UI display elements  
**Size**: ~400 lines of code

### Public Methods

#### `void Initialize(GameManager manager)`
Initialize UIManager and create all UI elements.

**Called By**: GameManager.Start()

**Example**:
```csharp
uiManager.Initialize(gameManager);
```

---

#### `void UpdateDisplay(int level, int lives)`
Update level and lives text display.

**Parameters**:
- `level` (int): Current level number
- `lives` (int): Current lives

**Example**:
```csharp
uiManager.UpdateDisplay(3, 2); // Shows "Level: 3" and "Lives: 2"
```

---

#### `void ShowMessage(string message)`
Display a temporary message to the player.

**Used For**:
- Level start messages
- Success/failure feedback
- Game status updates

**Example**:
```csharp
uiManager.ShowMessage("✓ Correct! Loading next round...");
uiManager.ShowMessage("✗ Wrong! Lives: 2");
```

---

#### `void ShowGameOverUI(System.Action onRestartClicked)`
Show game over screen with restart button.

**Parameters**:
- `onRestartClicked` (Action): Callback when restart clicked

**Called By**: GameManager.EndGame()

**Example**:
```csharp
uiManager.ShowGameOverUI(() => gameManager.RestartGame());
```

---

#### `void HideGameOverUI()`
Hide the game over screen.

**Called By**: GameManager.RestartGame()

**Example**:
```csharp
uiManager.HideGameOverUI();
```

---

### Private Methods

#### `private void CreateUI()`
Create all UI elements if they don't exist.

**Creates**:
- Canvas (Screen Space - Overlay)
- Level text (top-left)
- Lives text (top-right)
- Message text (center)
- Game over panel (centered)

---

#### `private Text CreateText(Canvas parent, string name, string content, Vector2 position, Vector2 size, int fontSize, TextAnchor alignment)`
Helper to create text UI element.

---

#### `private Image CreatePanel(Canvas parent, string name, Color color, Vector2 position, Vector2 size)`
Helper to create image/panel UI element.

---

#### `private Button CreateButton(GameObject parent, string name, string buttonText, Vector2 position, Vector2 size, int fontSize, Color buttonColor)`
Helper to create button UI element.

---

### UI Layout

```
┌─────────────────────────────────────────┐
│ Level: 1 (TL)      Ready?        Lives: 3 (TR)
│
│
│        ┌─────────────────────────┐
│        │  Tile  Tile  Tile  Tile │
│        │  Tile  Tile  Tile  Tile │
│        │  Tile  Tile  Tile  Tile │
│        │  Tile  Tile  Tile  Tile │
│        └─────────────────────────┘
│
│
├─────────────────────────────────────────┤
│ Game Over Overlay (Hidden by default)   │
│   "Game Over! You reached Level 3"       │
│   [Restart Game Button]                 │
└─────────────────────────────────────────┘
```

---

## SceneGenerator.cs

**Location**: `Assets/Scripts/SceneGenerator.cs`  
**Purpose**: Automatically generate the game scene in Unity Editor  
**Only works in Editor**: Uses #if UNITY_EDITOR preprocessor

### Menu Item

**Location**: Tools → Memory Grid → Generate Game Scene

**What It Does**:
1. Creates new scene named "MemoryGrid"
2. Sets up camera (dark blue background)
3. Creates GameManager GameObject and script
4. Creates GridManager GameObject and script
5. Creates UIManager GameObject and script
6. Sets default inspector values
7. Saves scene to Assets/Scenes/MemoryGrid.unity

### Usage

1. Open Memory Grid project in Unity
2. Menu: **Tools → Memory Grid → Generate Game Scene**
3. Wait for Console message: "✅ Memory Grid scene created successfully!"
4. Scene is ready to play!

---

## Code Style & Conventions

### Naming Conventions
- **Classes**: PascalCase (Tile, GridManager, GameManager)
- **Methods**: PascalCase (Initialize, Highlight, CreateGrid)
- **Variables**: camelCase (currentLevel, isHighlighted, tileGrid)
- **Constants**: UPPER_SNAKE_CASE (NORMAL_COLOR, HIGHLIGHT_COLOR)
- **Private members**: private with camelCase

### Comments
- All public methods have XML documentation (`/// <summary>`)
- Complex code has inline comments explaining logic
- Visual sections separated with `---` comments

### File Organization
```
Namespace (if used)

Using statements

Class declaration with XML documentation

Constants

Public properties

Component references (public SerializeField)

Game state variables

Initialization methods

Main game logic

Input handling

Getters/Accessors

Private helper methods
```

---

## Extending the Code

### Add Sound Effects
1. Create AudioManager.cs
2. Add AudioSource components
3. Call AudioManager.PlaySound("tileClick") in Tile.cs
4. Call AudioManager.PlaySound("success") in GameManager.cs

### Add Difficulty Levels
1. Modify GameManager to have difficulty enum
2. Adjust initialGridSize based on difficulty
3. Adjust patternDisplayTime based on difficulty

### Add High Scores
1. Create ScoreManager.cs
2. Serialize scores to PlayerPrefs
3. Display leaderboard in UIManager

### Add Power-ups
1. Create PowerUp prefab
2. Random spawn during gameplay
3. Pickup automatically gives extra life or pattern replay

---

## Debugging Tips

### Use Debug.Log() to Track Flow
```csharp
Debug.Log($"Pattern: {string.Join(",", currentPattern)}");
Debug.Log($"Player entered: {string.Join(",", playerPattern)}");
```

### Check State at Any Point
```csharp
Debug.Log($"Level {GetCurrentLevel()}, Lives: {GetCurrentLives()}, Grid: {GetGridSize()}");
```

### Monitor Game States
```csharp
if (!canPlayerClick) Debug.Log("Player cannot click yet!");
if (isShowingPattern) Debug.Log("Pattern is animating!");
```

---

## Performance Considerations

- **Grid Size**: Larger grids (6x6+) may slow down on older computers
- **Pattern Display Time**: Minimum 0.5 seconds recommended
- **Tile Count**: 16 (4x4) to 36 (6x6) tiles - all should be fast
- **Memory**: All UI created at runtime, minimal garbage collection

---

**End of Script Reference** 📚

