using UnityEngine;
using System.IO;

public class LocalDataManager : MonoBehaviour
{
    private string saveFileName = "echoes_save.json";
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);
    
    [Header("Debug")]
    public bool debugMode = true;
    
    public void SaveGameState(GameState gameState)
    {
        try
        {
            string json = JsonUtility.ToJson(gameState, true);
            File.WriteAllText(SavePath, json);
            
            if (debugMode)
                Debug.Log($"Game saved to: {SavePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }
    
    public GameState LoadGameState()
    {
        try
        {
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);
                GameState gameState = JsonUtility.FromJson<GameState>(json);
                
                if (debugMode)
                    Debug.Log($"Game loaded from: {SavePath}");
                    
                return gameState;
            }
            else
            {
                if (debugMode)
                    Debug.Log("No save file found, starting new game");
                    
                return null;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            return null;
        }
    }
    
    public void DeleteSave()
    {
        try
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
                Debug.Log("Save file deleted");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to delete save: {e.Message}");
        }
    }
    
    public bool HasSaveFile()
    {
        return File.Exists(SavePath);
    }
    
    // Quick save/load for testing
    [ContextMenu("Quick Save")]
    public void QuickSave()
    {
        if (GameManager.Instance?.currentGameState != null)
        {
            SaveGameState(GameManager.Instance.currentGameState);
        }
    }
    
    [ContextMenu("Quick Load")]
    public void QuickLoad()
    {
        GameState loadedState = LoadGameState();
        if (loadedState != null && GameManager.Instance != null)
        {
            GameManager.Instance.currentGameState = loadedState;
            GameManager.Instance.currentPlayer = loadedState.player;
        }
    }
    
    [ContextMenu("Delete Save")]
    public void DeleteSaveFile()
    {
        DeleteSave();
    }
    
    void Start()
    {
        if (debugMode)
        {
            Debug.Log($"Save path: {SavePath}");
            Debug.Log($"Has save file: {HasSaveFile()}");
        }
    }
}