using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Managers")]
    public GridManager gridManager;
    public TurnManager turnManager;
    public QuestManager questManager;
    public UIManager uiManager;
    public LocalDataManager dataManager;
    
    [Header("Game State")]
    public GameState currentGameState;
    public Player currentPlayer;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializeGame()
    {
        currentGameState = dataManager.LoadGameState();
        if (currentGameState == null)
        {
            currentGameState = new GameState();
            StartNewGame();
        }
        
        questManager.LoadQuest("sigil_of_eldara");
    }
    
    void StartNewGame()
    {
        currentPlayer = new Player
        {
            health = 100,
            maxHealth = 100,
            mana = 50,
            maxMana = 50,
            gridPosition = new Vector2Int(5, 5)
        };
        
        currentGameState.player = currentPlayer;
        currentGameState.currentQuest = "sigil_of_eldara";
        
        dataManager.SaveGameState(currentGameState);
    }
    
    public void SaveGame()
    {
        dataManager.SaveGameState(currentGameState);
    }
}