using UnityEngine;
using System.Collections;

/// <summary>
/// Main game manager: controls game state, levels, lives, and game flow.
/// Orchestrates interaction between GridManager and UIManager.
/// </summary>
public class GameManager : MonoBehaviour
{
    // --- Game Configuration ---
    [SerializeField] private int initialGridWidth = 4;      // Starting grid width
    [SerializeField] private int initialGridHeight = 4;     // Starting grid height
    [SerializeField] private int maxLives = 3;              // Starting lives per level
    [SerializeField] private float delayBetweenRounds = 2f; // Delay after pattern complete

    // --- Component References ---
    private GridManager gridManager;
    private UIManager uiManager;

    // --- Game State ---
    private int currentLevel = 1;           // Current level number
    private int currentLives;               // Current lives remaining
    private int currentGridWidth;           // Current grid width
    private int currentGridHeight;          // Current grid height
    private bool isLevelActive = false;     // Is a level currently in progress?
    private bool gameOver = false;          // Has player lost all lives?

    // --- Initialization ---

    /// <summary>
    /// Initialize the game manager and set up all components.
    /// Called once at game start.
    /// </summary>
    private void Start()
    {
        // Find or create managers
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager == null)
        {
            GameObject gridObj = new GameObject("GridManager");
            gridManager = gridObj.AddComponent<GridManager>();
        }

        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            GameObject uiObj = new GameObject("UIManager");
            uiManager = uiObj.AddComponent<UIManager>();
        }

        // Initialize managers
        gridManager.Initialize(this);
        uiManager.Initialize(this);

        // Start with initial state
        currentLives = maxLives;
        currentGridWidth = initialGridWidth;
        currentGridHeight = initialGridHeight;

        // Update UI and start first level
        uiManager.UpdateDisplay(currentLevel, currentLives);
        StartLevel();
    }

    // --- Level Management ---

    /// <summary>
    /// Start a new level.
    /// Creates the grid and launches the first round.
    /// </summary>
    private void StartLevel()
    {
        if (gameOver)
        {
            return;
        }

        isLevelActive = true;

        // Create the grid (or recreate if size changed)
        gridManager.ClearGrid();
        gridManager.CreateGrid();

        // Show level start message
        uiManager.ShowMessage($"Level {currentLevel} - Memory Grid {currentGridWidth}x{currentGridHeight}");

        // Start first round
        StartCoroutine(RunRound());
    }

    /// <summary>
    /// Main level loop: adds pattern element, shows it, waits for player input.
    /// </summary>
    private IEnumerator RunRound()
    {
        // Add a new element to the pattern (increases difficulty)
        gridManager.AddRandomPatternElement();

        // Show player the pattern
        yield return StartCoroutine(gridManager.ShowPattern());

        // Wait for player to complete or fail
        // This happens in OnPatternComplete() or OnWrongClick()
    }

    // --- Input Callbacks ---

    /// <summary>
    /// Called by GridManager when player completes the pattern correctly.
    /// </summary>
    public void OnPatternComplete()
    {
        if (!isLevelActive)
        {
            return;
        }

        uiManager.ShowMessage("✓ Correct! Loading next round...");

        // After a delay, start next round (with longer pattern)
        StartCoroutine(NextRoundAfterDelay());
    }

    /// <summary>
    /// Wait before starting next round.
    /// </summary>
    private IEnumerator NextRoundAfterDelay()
    {
        yield return new WaitForSeconds(delayBetweenRounds);
        StartCoroutine(RunRound());
    }

    /// <summary>
    /// Called by GridManager when player clicks a wrong tile.
    /// </summary>
    public void OnWrongClick()
    {
        if (!isLevelActive)
        {
            return;
        }

        currentLives--;
        uiManager.UpdateDisplay(currentLevel, currentLives);
        uiManager.ShowMessage($"✗ Wrong! Lives: {currentLives}");

        // Check if game is over
        if (currentLives <= 0)
        {
            EndGame();
        }
        else
        {
            // Restart the level with the same pattern
            StartCoroutine(RestartLevelAfterDelay());
        }
    }

    /// <summary>
    /// Restart the current level after a delay.
    /// </summary>
    private IEnumerator RestartLevelAfterDelay()
    {
        isLevelActive = false;
        yield return new WaitForSeconds(2f);
        currentLives = maxLives; // Reset lives for retry
        uiManager.UpdateDisplay(currentLevel, currentLives);
        StartLevel();
    }

    // --- Game Over / Progression ---

    /// <summary>
    /// End the game (player lost all lives).
    /// </summary>
    private void EndGame()
    {
        isLevelActive = false;
        gameOver = true;
        uiManager.ShowMessage($"Game Over! You reached Level {currentLevel}");
        uiManager.ShowGameOverUI(() => RestartGame());
    }

    /// <summary>
    /// Restart the entire game.
    /// </summary>
    public void RestartGame()
    {
        currentLevel = 1;
        currentLives = maxLives;
        gameOver = false;
        uiManager.UpdateDisplay(currentLevel, currentLives);
        uiManager.HideGameOverUI();
        StartLevel();
    }

    /// <summary>
    /// Progress to next level (increases difficulty).
    /// </summary>
    public void NextLevel()
    {
        currentLevel++;

        // Increase grid size every 3 levels
        if (currentLevel % 3 == 0)
        {
            if (currentGridWidth < 6)
            {
                currentGridWidth++;
                currentGridHeight++;
            }
        }

        currentLives = maxLives;
        uiManager.UpdateDisplay(currentLevel, currentLives);
        StartLevel();
    }

    // --- Getters ---

    /// <summary>
    /// Get the current level number.
    /// </summary>
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    /// <summary>
    /// Get the current lives remaining.
    /// </summary>
    public int GetCurrentLives()
    {
        return currentLives;
    }

    /// <summary>
    /// Check if the game is active.
    /// </summary>
    public bool IsGameActive()
    {
        return isLevelActive && !gameOver;
    }
}
