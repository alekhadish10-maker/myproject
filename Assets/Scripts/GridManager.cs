using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Manages the game grid: creating tiles, generating patterns, and checking player input.
/// Handles all grid-related logic including pattern display and pattern verification.
/// </summary>
public class GridManager : MonoBehaviour
{
    // --- Configurable Properties ---
    [SerializeField] private int gridWidth = 4;         // Horizontal grid size
    [SerializeField] private int gridHeight = 4;        // Vertical grid size
    [SerializeField] private float tileSize = 100f;     // Pixel size of each tile
    [SerializeField] private float spacing = 5f;        // Spacing between tiles
    [SerializeField] private float patternDisplayTime = 1.5f;   // How long to show each tile in pattern

    // --- Component References ---
    private GameManager gameManager;            // Reference to game manager
    private Tile[,] tileGrid;                   // 2D array of all tiles
    private RectTransform gridContainer;        // Container for positioning tiles
    private Tile tilePrefab;                    // Prefab for creating tiles

    // --- Pattern Management ---
    private List<Vector2Int> currentPattern;    // Pattern the player must recreate
    private List<Vector2Int> playerPattern;     // Pattern the player has entered so far
    private bool canPlayerClick = false;        // Whether player is allowed to click
    private bool isShowingPattern = false;      // Is pattern animation currently running?

    // --- Initialization ---

    /// <summary>
    /// Initialize the grid manager with required references and settings.
    /// </summary>
    /// <param name="manager">Reference to the game manager</param>
    public void Initialize(GameManager manager)
    {
        gameManager = manager;
        currentPattern = new List<Vector2Int>();
        playerPattern = new List<Vector2Int>();
    }

    /// <summary>
    /// Create the visual grid of tiles.
    /// Sets up the container and instantiates all tile GameObjects.
    /// </summary>
    public void CreateGrid()
    {
        // Create grid container if it doesn't exist
        if (gridContainer == null)
        {
            GameObject containerObj = new GameObject("GridContainer");
            containerObj.transform.parent = transform;
            gridContainer = containerObj.AddComponent<RectTransform>();
            gridContainer.anchoredPosition = Vector2.zero;
        }

        // Initialize the tile grid array
        tileGrid = new Tile[gridWidth, gridHeight];

        // Create each tile
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                CreateTile(x, y);
            }
        }

        // Center the grid on screen
        CenterGrid();
    }

    /// <summary>
    /// Create a single tile at the specified grid position.
    /// </summary>
    private void CreateTile(int x, int y)
    {
        // Create new tile GameObject
        GameObject tileObj = new GameObject($"Tile_{x}_{y}");
        tileObj.transform.parent = gridContainer.transform;

        // Set up RectTransform for positioning
        RectTransform rectTransform = tileObj.AddComponent<RectTransform>();
        float xPos = x * (tileSize + spacing);
        float yPos = -y * (tileSize + spacing); // Negative Y because UI grows downward
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform.sizeDelta = new Vector2(tileSize, tileSize);

        // Add Image component for visual representation
        Image image = tileObj.AddComponent<Image>();
        image.color = new Color(0.7f, 0.7f, 0.7f); // Gray color

        // Add Button component for interaction
        Button button = tileObj.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.7f, 0.7f, 0.7f);
        colors.highlightedColor = new Color(0.6f, 0.6f, 0.6f);
        colors.pressedColor = new Color(0.5f, 0.5f, 0.5f);
        button.colors = colors;

        // Add Tile script and initialize it
        Tile tile = tileObj.AddComponent<Tile>();
        tile.Initialize(x, y, this);

        // Store reference in grid array
        tileGrid[x, y] = tile;
    }

    /// <summary>
    /// Center the grid on the screen by adjusting container position.
    /// </summary>
    private void CenterGrid()
    {
        float totalWidth = (gridWidth - 1) * (tileSize + spacing);
        float totalHeight = (gridHeight - 1) * (tileSize + spacing);

        gridContainer.anchoredPosition = new Vector2(-totalWidth / 2, totalHeight / 2);
    }

    // --- Pattern Management ---

    /// <summary>
    /// Generate a new random pattern element and add it to the current pattern.
    /// Called when moving to the next level or starting a new round.
    /// </summary>
    public void AddRandomPatternElement()
    {
        int randomX = Random.Range(0, gridWidth);
        int randomY = Random.Range(0, gridHeight);
        currentPattern.Add(new Vector2Int(randomX, randomY));
    }

    /// <summary>
    /// Display the current pattern to the player.
    /// Shows each tile in sequence with a delay between them.
    /// </summary>
    public IEnumerator ShowPattern()
    {
        canPlayerClick = false;
        isShowingPattern = true;

        // Add a small delay before starting
        yield return new WaitForSeconds(0.5f);

        // Show each tile in the pattern
        foreach (Vector2Int tilePos in currentPattern)
        {
            // Highlight the tile
            tileGrid[tilePos.x, tilePos.y].Highlight();
            yield return new WaitForSeconds(patternDisplayTime);

            // Unhighlight the tile
            tileGrid[tilePos.x, tilePos.y].Unhighlight();
            yield return new WaitForSeconds(0.3f); // Small pause between tiles
        }

        isShowingPattern = false;
        
        // Player is now allowed to click
        canPlayerClick = true;
    }

    /// <summary>
    /// Clear all player input and reset visuals.
    /// </summary>
    public void ResetPlayerPattern()
    {
        playerPattern.Clear();
        
        // Unhighlight all tiles
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                tileGrid[x, y].Unhighlight();
            }
        }
    }

    /// <summary>
    /// Get the current pattern length (difficulty).
    /// </summary>
    public int GetPatternLength()
    {
        return currentPattern.Count;
    }

    // --- Input Handling ---

    /// <summary>
    /// Called when a tile is clicked by the player.
    /// Verifies if the click matches the pattern.
    /// </summary>
    public void OnTileClicked(int x, int y)
    {
        // Ignore clicks when player shouldn't be clicking
        if (!canPlayerClick || isShowingPattern)
        {
            return;
        }

        Vector2Int clickedPos = new Vector2Int(x, y);
        playerPattern.Add(clickedPos);

        // Check if this click is correct
        if (clickedPos != currentPattern[playerPattern.Count - 1])
        {
            // WRONG CLICK - Tell game manager
            gameManager.OnWrongClick();
            tileGrid[x, y].FlashError();
            return;
        }

        // Correct click - keep tile highlighted
        // Check if player has completed the pattern
        if (playerPattern.Count == currentPattern.Count)
        {
            // PATTERN COMPLETE - Tell game manager
            gameManager.OnPatternComplete();
            canPlayerClick = false;
        }
    }

    /// <summary>
    /// Clear and disable all tiles (for cleanup).
    /// </summary>
    public void ClearGrid()
    {
        if (tileGrid != null)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    if (tileGrid[x, y] != null)
                    {
                        Destroy(tileGrid[x, y].gameObject);
                    }
                }
            }
        }
        tileGrid = null;
    }

    /// <summary>
    /// Get grid dimensions for debugging or UI display.
    /// </summary>
    public Vector2Int GetGridSize()
    {
        return new Vector2Int(gridWidth, gridHeight);
    }

    /// <summary>
    /// Check if the player can currently click tiles.
    /// </summary>
    public bool CanPlayerClick()
    {
        return canPlayerClick;
    }
}
