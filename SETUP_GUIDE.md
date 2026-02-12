# Memory Grid - Complete Setup Guide

This guide walks you through everything needed to run the Memory Grid game in Unity.

## Table of Contents
1. [Project Setup](#project-setup)
2. [Automatic Scene Generation](#automatic-scene-generation)
3. [Manual Setup (If Needed)](#manual-setup-if-needed)
4. [Script Details](#script-details)
5. [Testing & Debugging](#testing--debugging)
6. [Common Issues](#common-issues)

---

## Project Setup

### Step 1: Download and Install Unity
1. Go to [unity.com/download](https://unity.com/download)
2. Download **Unity Hub**
3. Install Unity Hub
4. Open Unity Hub
5. Install **Unity 2020.3 LTS or later**

### Step 2: Open the Project
1. In Unity Hub, click **"Add Project from Disk"**
2. Navigate to: `c:\Users\10alha05\.vscode\projektgame`
3. Click **"Select Folder"**
4. Wait for Unity to open and process the project
5. This may take 1-2 minutes on first load

### Step 3: Verify Scripts Loaded
1. In Project panel, navigate to: `Assets > Scripts`
2. You should see these files:
   - ✅ Tile.cs
   - ✅ GridManager.cs
   - ✅ GameManager.cs
   - ✅ UIManager.cs
   - ✅ SceneGenerator.cs

If any are missing, check the directory exists: `c:\Users\10alha05\.vscode\projektgame\Assets\Scripts\`

---

## Automatic Scene Generation (EASIEST WAY)

### ✨ The Fast Method (Recommended)

1. **Open Unity Editor** with the Memory Grid project
2. In the menu bar, click: **Tools → Memory Grid → Generate Game Scene**
3. Wait for confirmation in the Console window
4. A new scene **MemoryGrid.unity** is created automatically

### What Gets Generated
✅ Canvas with all UI elements  
✅ GameManager script attached  
✅ GridManager script attached  
✅ UIManager script attached  
✅ All UI texts (Level, Lives, Messages)  
✅ Game Over panel with restart button  

### After Generation
1. Scene is saved to: `Assets/Scenes/MemoryGrid.unity`
2. Click the **Play button** (▶️) in the toolbar
3. Game starts! 🎉

---

## Manual Setup (If Needed)

Use this only if auto-generation doesn't work.

### Step 1: Create Scene
1. Right-click in **Project panel** under `Assets/Scenes/`
2. **Create > Scene**
3. Name it: `MemoryGrid`
4. Double-click to open

### Step 2: Setup Camera
1. Select **Main Camera** in Hierarchy
2. In Inspector, set:
   - **Clear Flags**: Solid Color
   - **Background Color**: `(0.1, 0.1, 0.15)` (Dark blue)
   - **Orthographic Size**: 6

### Step 3: Create GameManager
1. Right-click in Hierarchy
2. **Create Empty**
3. Name it: `GameManager`
4. Drag **GameManager.cs** onto this GameObject
5. In Inspector, set:
   - Initial Grid Width: 4
   - Initial Grid Height: 4
   - Max Lives: 3
   - Delay Between Rounds: 2

### Step 4: Create GridManager
1. Right-click in Hierarchy
2. **Create Empty**
3. Name it: `GridManager`
4. Drag **GridManager.cs** onto this GameObject
5. In Inspector, set:
   - Grid Width: 4
   - Grid Height: 4
   - Tile Size: 100
   - Spacing: 5
   - Pattern Display Time: 1.5

### Step 5: Create UIManager
1. Right-click in Hierarchy
2. **Create Empty**
3. Name it: `UIManager`
4. Drag **UIManager.cs** onto this GameObject
5. UIManager will create all UI elements automatically

### Step 6: Save Scene
1. **Ctrl + S** (or File > Save)
2. Make sure it's saved as: `Assets/Scenes/MemoryGrid.unity`

---

## Script Details

### Tile.cs
**Purpose**: Represents a single grid tile

**Key Methods**:
- `Initialize(x, y, manager)` - Set up tile position
- `Highlight()` - Turn tile on (bright cyan)
- `Unhighlight()` - Turn tile off (gray)
- `FlashError()` - Red flash on wrong click

**Key Variables**:
- `gridX`, `gridY` - Position in grid
- `isHighlighted` - Current state
- `tileImage` - UI Image component

**How It Works**:
1. Created by GridManager
2. When clicked, notifies GridManager
3. Visually highlights/unhighlights based on game state

### GridManager.cs
**Purpose**: Manages the game grid and pattern logic

**Key Methods**:
- `CreateGrid()` - Build all tiles
- `AddRandomPatternElement()` - Add one tile to the pattern
- `ShowPattern()` - Animate pattern display to player
- `OnTileClicked(x, y)` - Handle player click
- `ResetPlayerPattern()` - Clear player's entered pattern

**Key Variables**:
- `tileGrid[,]` - 2D array of all tiles
- `currentPattern` - The pattern to recreate
- `playerPattern` - What player has entered so far
- `canPlayerClick` - Is clicking allowed?

**How It Works**:
1. Creates a 2D grid of Tiles
2. Generates random patterns
3. Shows patterns with delays between each tile
4. Validates player clicks against pattern
5. Detects when pattern is complete or incorrect

### GameManager.cs
**Purpose**: Main game controller and state manager

**Key Methods**:
- `StartLevel()` - Initialize a new level
- `OnPatternComplete()` - Called when player completes pattern
- `OnWrongClick()` - Called when player clicks wrong tile
- `RestartGame()` - Reset to level 1
- `NextLevel()` - Advance to next level

**Key Variables**:
- `currentLevel` - Current level number
- `currentLives` - Lives remaining
- `isLevelActive` - Is a level active?
- `gameOver` - Has game ended?

**Game Flow**:
1. Start Level → Create grid → Add pattern element
2. Show Pattern → Wait for player
3. On Correct Click → Add new pattern element → Go to Step 2
4. On Wrong Click → Lose life → Restart or Game Over

### UIManager.cs
**Purpose**: Create and manage all user interface elements

**Key Methods**:
- `CreateUI()` - Generate all UI elements
- `UpdateDisplay(level, lives)` - Update level/lives text
- `ShowMessage(text)` - Display temporary message
- `ShowGameOverUI(callback)` - Show game over screen
- `HideGameOverUI()` - Hide game over screen

**Key Variables**:
- `levelText` - Displays current level
- `livesText` - Displays remaining lives
- `messageText` - Shows game messages
- `restartButton` - Button to restart game

**UI Layout**:
- Top-left: Level display
- Top-right: Lives display
- Top-center: Message text
- Center (game over): Game over panel with restart button

---

## Testing & Debugging

### Basic Testing
After generating the scene:

1. **Press Play** (▶️ button)
2. **Observe**:
   - Title bar shows "Level: 1"
   - Lives counter shows "3"
   - Message says "Ready?"
   - First tile highlights in cyan
3. **Click** the highlighted tile
4. It should stay highlighted
5. If pattern has 2 tiles, second tile highlights
6. Click it to complete the pattern
7. Next round starts with longer pattern

### Check Console for Errors
1. Window → General → Console (or Ctrl + Shift + C)
2. Watch for red error messages
3. Common errors and fixes:

```
NullReferenceException in GridManager.OnTileClicked
→ Check GridManager is assigned in GameManager

NullReferenceException in UIManager.UpdateDisplay
→ UIManager may not be created - run SceneGenerator again

Tile not responding to clicks
→ Check Button component exists on tile
→ Check Canvas Render Mode is "Screen Space - Overlay"
```

### Step Through the Game
1. Start playing
2. Don't click anything - just watch pattern
3. When all highlights finish, try clicking one
4. Test each tile by clicking in wrong order
5. Notice the red error flash
6. Restart and complete correctly

---

## Common Issues & Fixes

### Issue: Scene Generator Menu Doesn't Appear
**Solution**:
1. Make sure scripts are in: `Assets/Scripts/`
2. Wait for Unity to recompile (watch status bar)
3. Restart Unity if still missing
4. Check Console for any compilation errors

### Issue: Nothing Happens When I Click Play
**Solution**:
1. Make sure scene is saved
2. Check that **MemoryGrid.unity** is open (not greyed out in Project)
3. Try: File → Open Scene → MemoryGrid.unity
4. Check Console for errors

### Issue: Tiles Don't Light Up
**Solution**:
1. Select a Tile in Hierarchy
2. In Inspector, check Image component exists
3. Verify colors are set (not invisible white-on-white)
4. Check Canvas Render Mode is "Screen Space - Overlay"

### Issue: Game Doesn't Track Lives/Level
**Solution**:
1. Verify UIManager is in Hierarchy
2. Check UIManager script has no errors in Console
3. Verify GameManager script is attached to "GameManager" GameObject
4. Try regenerating scene with SceneGenerator

### Issue: Pattern Can't Be Clicked
**Solution**:
1. Pattern must finish displaying first (wait 1-2 seconds)
2. Check message says "Ready?" before clicking
3. Verify canPlayerClick is true in GridManager
4. Try clicking with left mouse button (not right)

### Issue: Game Freezes or Lags
**Solution**:
1. Check Console for infinite loops
2. Verify patternDisplayTime > 0 (not negative)
3. Make sure grid size is ≤ 6x6 (larger grids slow down)
4. Close other heavy applications

---

## Quick Test Checklist

- [ ] Unity project opens without errors
- [ ] Tools > Memory Grid > Generate Game Scene works
- [ ] MemoryGrid.unity scene created
- [ ] Pressing Play shows the game
- [ ] First tile lights up cyan
- [ ] Clicking tile keeps it lit
- [ ] Completing pattern advances round
- [ ] Wrong click shows error (red flash)
- [ ] Lives counter decrements on error
- [ ] Game restarts after losing all lives

---

## Performance Tips

**For Slower Computers**:
1. Reduce grid size: 3x3 instead of 4x4
2. Increase delays: patternDisplayTime = 2.5 instead of 1.5
3. Close other applications in background

**For Better Experience**:
1. Increase tile size: 120 instead of 100
2. Decrease pattern display time: 1.0 instead of 1.5
3. Use 5x5 grid for more challenge

---

## Next Steps

1. **Play and Enjoy**: Test the game!
2. **Customize**: Change colors, grid size, difficulty
3. **Extend**: Add sound effects, music, leaderboard
4. **Share**: Build and share with friends
5. **Learn**: Study the code, understand the patterns

---

**Stuck?** Double-check that:
1. All scripts are in `Assets/Scripts/`
2. Scene was generated or set up correctly
3. No red errors in Console
4. Canvas Render Mode is correct
5. Main Camera is set to Orthographic

**Happy memory puzzling!** 🧠

