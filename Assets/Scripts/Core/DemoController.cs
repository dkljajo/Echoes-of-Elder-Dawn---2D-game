using UnityEngine;
using System.Collections;

public class DemoController : MonoBehaviour
{
    [Header("Demo Settings")]
    public bool autoDemo = false;
    public float demoStepDelay = 3f;
    
    [Header("Demo UI")]
    public GameObject demoPanel;
    public TMPro.TextMeshProUGUI demoText;
    public UnityEngine.UI.Button nextStepButton;
    public UnityEngine.UI.Button skipDemoButton;
    
    private int currentDemoStep = 0;
    private string[] demoSteps = {
        "Welcome to Echoes of Elder Dawn!\nA turn-based RPG set in the fantasy realm of Elarion.",
        "Use WASD or Arrow Keys to move around the grid-based world.",
        "This is Solendra, the lush mountainous region where your adventure begins.",
        "Move to the ruins at position (8,8) to discover the Sigil of Eldara quest.",
        "Press Q to open the quest menu and see available quests.",
        "Press E to interact with NPCs and objects in the world.",
        "Experience turn-based combat with 4 actions: Attack, Skill, Defend, Use Item.",
        "Make meaningful choices that affect the story and world.",
        "See AI-generated quests that adapt to your decisions (simulated locally).",
        "This demo shows the core gameplay loop ready for AWS integration!"
    };
    
    void Start()
    {
        SetupDemo();
        
        if (autoDemo)
        {
            StartCoroutine(AutoDemoSequence());
        }
    }
    
    void SetupDemo()
    {
        if (demoPanel != null)
        {
            demoPanel.SetActive(true);
            UpdateDemoText();
            
            if (nextStepButton != null)
                nextStepButton.onClick.AddListener(NextDemoStep);
                
            if (skipDemoButton != null)
                skipDemoButton.onClick.AddListener(EndDemo);
        }
    }
    
    void UpdateDemoText()
    {
        if (demoText != null && currentDemoStep < demoSteps.Length)
        {
            demoText.text = $"Demo Step {currentDemoStep + 1}/{demoSteps.Length}\n\n{demoSteps[currentDemoStep]}";
        }
    }
    
    public void NextDemoStep()
    {
        currentDemoStep++;
        
        if (currentDemoStep >= demoSteps.Length)
        {
            EndDemo();
            return;
        }
        
        UpdateDemoText();
        ExecuteDemoStep();
    }
    
    void ExecuteDemoStep()
    {
        switch (currentDemoStep)
        {
            case 1: // Movement demo
                HighlightMovementControls();
                break;
                
            case 3: // Quest location
                HighlightRuinsLocation();
                break;
                
            case 4: // Quest menu
                ShowQuestMenuDemo();
                break;
                
            case 6: // Combat demo
                TriggerDemoCombat();
                break;
                
            case 8: // AI quest demo
                ShowAIQuestDemo();
                break;
        }
    }
    
    void HighlightMovementControls()
    {
        Debug.Log("Demo: Try moving with WASD or Arrow Keys!");
    }
    
    void HighlightRuinsLocation()
    {
        // Highlight the ruins location on the map
        Vector2Int ruinsPos = new Vector2Int(8, 8);
        Vector3 worldPos = GameManager.Instance.gridManager.GridToWorldPosition(ruinsPos);
        
        // Create a visual indicator (you could add a glowing effect here)
        Debug.Log($"Demo: Ruins location highlighted at {worldPos}");
    }
    
    void ShowQuestMenuDemo()
    {
        // Automatically open quest menu
        GameManager.Instance.uiManager.ShowQuestUI(true);
        
        // Load the main quest for demonstration
        GameManager.Instance.questManager.LoadQuest("sigil_of_eldara");
    }
    
    void TriggerDemoCombat()
    {
        // Create a demo enemy for combat
        GameObject enemyObj = new GameObject("Demo Enemy");
        CombatEntity enemy = enemyObj.AddComponent<CombatEntity>();
        enemy.Initialize("Demo Guardian", 40, 40);
        
        var combatants = new System.Collections.Generic.List<CombatEntity>
        {
            GameManager.Instance.currentPlayer,
            enemy
        };
        
        GameManager.Instance.turnManager.StartCombat(combatants);
    }
    
    void ShowAIQuestDemo()
    {
        // Demonstrate AI quest generation
        LocalAIGenerator aiGen = FindObjectOfType<LocalAIGenerator>();
        if (aiGen != null)
        {
            QuestTemplate aiQuest = aiGen.GetAIQuest("demo showcase");
            
            // Show the AI-generated quest
            GameManager.Instance.uiManager.UpdateQuestUI(aiQuest);
            
            Debug.Log("Demo: AI-generated quest displayed!");
        }
    }
    
    IEnumerator AutoDemoSequence()
    {
        while (currentDemoStep < demoSteps.Length)
        {
            yield return new WaitForSeconds(demoStepDelay);
            NextDemoStep();
        }
    }
    
    public void EndDemo()
    {
        if (demoPanel != null)
        {
            demoPanel.SetActive(false);
        }
        
        Debug.Log("Demo completed! Game is now in free-play mode.");
        
        // Enable full game functionality
        EnableFullGame();
    }
    
    void EnableFullGame()
    {
        // Ensure all systems are active
        GameManager.Instance.questManager.LoadQuest("sigil_of_eldara");
        
        // Show helpful UI
        GameManager.Instance.uiManager.ShowQuestOutcome(
            "Demo Complete!\n\n" +
            "Controls:\n" +
            "WASD/Arrows - Move\n" +
            "Q - Quest Menu\n" +
            "E - Interact\n\n" +
            "Explore the world of Elarion!"
        );
    }
    
    void Update()
    {
        // Skip demo with Escape key
        if (Input.GetKeyDown(KeyCode.Escape) && demoPanel != null && demoPanel.activeInHierarchy)
        {
            EndDemo();
        }
    }
    
    // Public methods for external triggers
    public void StartDemo()
    {
        currentDemoStep = 0;
        SetupDemo();
    }
    
    public void PauseDemo()
    {
        StopAllCoroutines();
    }
    
    public void ResumeDemo()
    {
        if (autoDemo)
        {
            StartCoroutine(AutoDemoSequence());
        }
    }
}