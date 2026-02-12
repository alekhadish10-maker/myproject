using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Manages all UI display and user interface elements.
/// Handles level display, lives counter, messages, and game over screen.
/// </summary>
public class UIManager : MonoBehaviour
{
    // --- Component References ---
    private GameManager gameManager;
    private Canvas mainCanvas;
    private Text levelText;
    private Text livesText;
    private Text messageText;
    private Button restartButton;
    private Image gameOverPanel;

    // --- Initialization ---

    /// <summary>
    /// Initialize the UI manager and create all UI elements.
    /// </summary>
    public void Initialize(GameManager manager)
    {
        gameManager = manager;
        CreateUI();
    }

    /// <summary>
    /// Create all UI elements if they don't exist.
    /// </summary>
    private void CreateUI()
    {
        // Find or create canvas
        mainCanvas = FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            mainCanvas = canvasObj.AddComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
        }

        // Create Level Text
        levelText = CreateText(mainCanvas, "LevelText", "Level: 1", 
            new Vector2(100, -50), new Vector2(300, 60), 24, TextAnchor.MiddleLeft);

        // Create Lives Text
        livesText = CreateText(mainCanvas, "LivesText", "Lives: 3", 
            new Vector2(-100, -50), new Vector2(300, 60), 24, TextAnchor.MiddleRight);

        // Create Message Text (center top)
        messageText = CreateText(mainCanvas, "MessageText", "Ready?", 
            new Vector2(0, -100), new Vector2(800, 100), 32, TextAnchor.MiddleCenter);
        messageText.color = Color.cyan;

        // Create Game Over Panel
        gameOverPanel = CreatePanel(mainCanvas, "GameOverPanel", 
            new Color(0, 0, 0, 0.7f), new Vector2(0, 0), new Vector2(1920, 1080));
        gameOverPanel.gameObject.SetActive(false);

        // Create Game Over Text
        Text gameOverText = CreateText(gameOverPanel.gameObject, "GameOverText", "Game Over!", 
            new Vector2(0, 100), new Vector2(600, 150), 48, TextAnchor.MiddleCenter);
        gameOverText.color = Color.red;

        // Create Restart Button
        restartButton = CreateButton(gameOverPanel.gameObject, "RestartButton", "Restart Game", 
            new Vector2(0, -50), new Vector2(300, 80), 28, Color.green);
    }

    // --- UI Creation Helpers ---

    /// <summary>
    /// Create a text element and add it to a parent.
    /// </summary>
    private Text CreateText(Canvas parent, string name, string content, 
        Vector2 position, Vector2 size, int fontSize, TextAnchor alignment)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform, false);

        RectTransform rectTransform = textObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;

        Text text = textObj.AddComponent<Text>();
        text.text = content;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.fontStyle = FontStyle.Bold;
        text.alignment = alignment;
        text.color = Color.white;

        return text;
    }

    /// <summary>
    /// Create a text element as a child of a GameObject.
    /// </summary>
    private Text CreateText(GameObject parent, string name, string content, 
        Vector2 position, Vector2 size, int fontSize, TextAnchor alignment)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform, false);

        RectTransform rectTransform = textObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;

        Text text = textObj.AddComponent<Text>();
        text.text = content;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.fontStyle = FontStyle.Bold;
        text.alignment = alignment;
        text.color = Color.white;

        return text;
    }

    /// <summary>
    /// Create an image panel.
    /// </summary>
    private Image CreatePanel(Canvas parent, string name, Color color, Vector2 position, Vector2 size)
    {
        GameObject panelObj = new GameObject(name);
        panelObj.transform.SetParent(parent.transform, false);

        RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;

        Image image = panelObj.AddComponent<Image>();
        image.color = color;

        return image;
    }

    /// <summary>
    /// Create a button with text.
    /// </summary>
    private Button CreateButton(GameObject parent, string name, string buttonText, 
        Vector2 position, Vector2 size, int fontSize, Color buttonColor)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent.transform, false);

        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;

        Image image = buttonObj.AddComponent<Image>();
        image.color = buttonColor;

        Button button = buttonObj.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = buttonColor;
        colors.highlightedColor = buttonColor * 1.2f;
        colors.pressedColor = buttonColor * 0.8f;
        button.colors = colors;

        // Create button text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text text = textObj.AddComponent<Text>();
        text.text = buttonText;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        return button;
    }

    // --- UI Updates ---

    /// <summary>
    /// Update the display of level and lives.
    /// </summary>
    public void UpdateDisplay(int level, int lives)
    {
        if (levelText != null)
            levelText.text = $"Level: {level}";

        if (livesText != null)
            livesText.text = $"Lives: {lives}";
    }

    /// <summary>
    /// Show a temporary message to the player.
    /// </summary>
    public void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
    }

    /// <summary>
    /// Show the game over UI panel.
    /// </summary>
    public void ShowGameOverUI(System.Action onRestartClicked)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.gameObject.SetActive(true);
            
            if (restartButton != null)
            {
                restartButton.onClick.AddListener(() => onRestartClicked?.Invoke());
            }
        }
    }

    /// <summary>
    /// Hide the game over UI panel.
    /// </summary>
    public void HideGameOverUI()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.gameObject.SetActive(false);
        }
    }
}
