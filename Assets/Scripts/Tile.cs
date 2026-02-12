using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls individual tile behavior and visual state.
/// Each tile can be highlighted, clicked, and has a specific position in the grid.
/// </summary>
public class Tile : MonoBehaviour
{
    // --- Public Properties ---
    /// <summary>The column position of this tile in the grid (0 to gridWidth-1)</summary>
    public int gridX { get; private set; }
    
    /// <summary>The row position of this tile in the grid (0 to gridHeight-1)</summary>
    public int gridY { get; private set; }

    // --- Private Properties ---
    private Image tileImage;                    // UI Image component for visual representation
    private Button tileButton;                  // Button component for click detection
    private Color originalColor;                // Original tile color when not highlighted
    private Color highlightedColor;             // Color when tile is highlighted/lit up
    private bool isHighlighted = false;         // Is this tile currently highlighted?
    private GridManager gridManager;            // Reference to the grid manager
    
    // Color constants
    private static readonly Color NORMAL_COLOR = new Color(0.7f, 0.7f, 0.7f);       // Gray
    private static readonly Color HIGHLIGHT_COLOR = new Color(0.2f, 0.8f, 1f);      // Cyan

    // --- Initialization ---

    /// <summary>
    /// Initialize this tile with its grid position and references.
    /// Called by GridManager when creating the tile.
    /// </summary>
    /// <param name="x">Column position in grid</param>
    /// <param name="y">Row position in grid</param>
    /// <param name="manager">Reference to GridManager</param>
    public void Initialize(int x, int y, GridManager manager)
    {
        gridX = x;
        gridY = y;
        gridManager = manager;

        // Get UI components
        tileImage = GetComponent<Image>();
        tileButton = GetComponent<Button>();

        // Set initial colors
        originalColor = NORMAL_COLOR;
        highlightedColor = HIGHLIGHT_COLOR;
        
        // Set tile to normal state
        ResetVisuals();

        // Connect button click event
        tileButton.onClick.AddListener(OnTileClicked);
    }

    // --- Visual State Management ---

    /// <summary>
    /// Highlight this tile (turn it on with bright color).
    /// Used when showing the pattern or confirming correct clicks.
    /// </summary>
    public void Highlight()
    {
        if (tileImage != null)
        {
            tileImage.color = highlightedColor;
            isHighlighted = true;
        }
    }

    /// <summary>
    /// Remove highlight from this tile (return to normal color).
    /// </summary>
    public void Unhighlight()
    {
        if (tileImage != null)
        {
            tileImage.color = originalColor;
            isHighlighted = false;
        }
    }

    /// <summary>
    /// Reset tile visuals to initial state.
    /// </summary>
    public void ResetVisuals()
    {
        if (tileImage != null)
        {
            tileImage.color = originalColor;
            isHighlighted = false;
        }
    }

    /// <summary>
    /// Flash this tile for wrong click feedback.
    /// Shows red color briefly to indicate error.
    /// </summary>
    public void FlashError()
    {
        StartCoroutine(ErrorFlashCoroutine());
    }

    /// <summary>
    /// Coroutine that performs error flash animation.
    /// </summary>
    private System.Collections.IEnumerator ErrorFlashCoroutine()
    {
        Color errorColor = new Color(1f, 0.2f, 0.2f); // Red
        tileImage.color = errorColor;
        yield return new WaitForSeconds(0.3f);
        tileImage.color = originalColor;
    }

    /// <summary>
    /// Check if this tile is currently highlighted.
    /// </summary>
    public bool IsHighlighted()
    {
        return isHighlighted;
    }

    // --- Input Handling ---

    /// <summary>
    /// Called when this tile's button is clicked.
    /// Notifies GridManager of the click with this tile's position.
    /// </summary>
    private void OnTileClicked()
    {
        // Immediately highlight the tile for visual feedback
        Highlight();

        // Tell the grid manager this tile was clicked
        gridManager.OnTileClicked(gridX, gridY);
    }

    /// <summary>
    /// Get the grid position of this tile as a string for debugging.
    /// </summary>
    public override string ToString()
    {
        return $"Tile({gridX}, {gridY})";
    }
}
