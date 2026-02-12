using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

/// <summary>
/// Automatically generates the Memory Grid game scene in the Unity Editor.
/// Use: Tools → Memory Grid → Generate Scene
/// </summary>
public class SceneGenerator : MonoBehaviour
{
    #if UNITY_EDITOR
    [MenuItem("Tools/Memory Grid/Generate Game Scene")]
    public static void GenerateScene()
    {
        // Create new scene
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneCreateCallback.Save);
        scene.name = "MemoryGrid";

        // Setup camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = new Color(0.1f, 0.1f, 0.15f); // Dark blue
        }

        // Create GameManager
        GameObject gmObj = new GameObject("GameManager");
        GameManager gameManager = gmObj.AddComponent<GameManager>();
        gameManager.gameObject.name = "GameManager";

        // Assign default values in inspector if needed
        SerializedObject so = new SerializedObject(gameManager);
        so.FindProperty("initialGridWidth").intValue = 4;
        so.FindProperty("initialGridHeight").intValue = 4;
        so.FindProperty("maxLives").intValue = 3;
        so.ApplyModifiedProperties();

        // Create GridManager
        GameObject gridObj = new GameObject("GridManager");
        GridManager gridManager = gridObj.AddComponent<GridManager>();

        // Create UIManager
        GameObject uiObj = new GameObject("UIManager");
        UIManager uiManager = uiObj.AddComponent<UIManager>();

        // Save scene
        EditorSceneManager.SaveScene(scene);

        Debug.Log("✅ Memory Grid scene created successfully!");
        Debug.Log("📍 Scene saved as: MemoryGrid.unity");
        Debug.Log("🎮 Press Play to start the game!");
    }

    [MenuItem("Tools/Memory Grid/Generate Game Scene", validate = true)]
    public static bool ValidateGenerateScene()
    {
        // Menu item is available in editor only
        return true;
    }
    #endif
}
