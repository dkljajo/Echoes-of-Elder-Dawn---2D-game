using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoSetup : MonoBehaviour
{
    [Header("Auto Setup")]
    public bool setupOnStart = true;
    
    void Start()
    {
        if (setupOnStart)
        {
            SetupCompleteGame();
        }
    }
    
    [ContextMenu("Setup Complete Game")]
    public void SetupCompleteGame()
    {
        Debug.Log("Starting automatic game setup...");
        
        CreateManagers();
        CreatePlayer();
        CreateUI();
        CreateGrid();
        ConnectReferences();
        
        Debug.Log("Game setup complete! Ready to play!");
    }
    
    void CreateManagers()
    {
        // GameManager
        GameObject gameManagerObj = new GameObject("GameManager");
        gameManagerObj.AddComponent<GameManager>();
        gameManagerObj.AddComponent<LocalDataManager>();
        
        // Other Managers
        GameObject gridManagerObj = new GameObject("GridManager");
        gridManagerObj.AddComponent<GridManager>();
        
        GameObject turnManagerObj = new GameObject("TurnManager");
        turnManagerObj.AddComponent<TurnManager>();
        
        GameObject questManagerObj = new GameObject("QuestManager");
        questManagerObj.AddComponent<QuestManager>();
        questManagerObj.AddComponent<LocalAIGenerator>();
        
        GameObject uiManagerObj = new GameObject("UIManager");
        uiManagerObj.AddComponent<UIManager>();
        
        GameObject demoObj = new GameObject("DemoController");
        demoObj.AddComponent<DemoController>();
    }
    
    void CreatePlayer()
    {
        GameObject player = new GameObject("Player");
        player.AddComponent<SpriteRenderer>();
        player.AddComponent<PlayerController>();
        player.AddComponent<BoxCollider2D>();
        player.transform.position = new Vector3(5, 5, 0);
        player.tag = "Player";
    }
    
    void CreateUI()
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Health Bar
        CreateSlider(canvasObj, "HealthBar", new Vector2(-800, 400));
        
        // Mana Bar  
        CreateSlider(canvasObj, "ManaBar", new Vector2(-800, 350));
        
        // Combat Panel
        CreateCombatPanel(canvasObj);
        
        // Quest Panel
        CreateQuestPanel(canvasObj);
        
        // Dialogue Panel
        CreateDialoguePanel(canvasObj);
        
        // Outcome Panel
        CreateOutcomePanel(canvasObj);
    }
    
    void CreateGrid()
    {
        GameObject gridObj = new GameObject("Grid");
        gridObj.AddComponent<Grid>();
        
        GameObject tilemapObj = new GameObject("Tilemap");
        tilemapObj.transform.SetParent(gridObj.transform);
        tilemapObj.AddComponent<Tilemap>();
        tilemapObj.AddComponent<TilemapRenderer>();
    }
    
    GameObject CreateSlider(GameObject parent, string name, Vector2 position)
    {
        GameObject sliderObj = new GameObject(name);
        sliderObj.transform.SetParent(parent.transform);
        
        RectTransform rect = sliderObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(200, 20);
        
        Slider slider = sliderObj.AddComponent<Slider>();
        slider.value = 1f;
        
        // Background
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(sliderObj.transform);
        bg.AddComponent<Image>().color = Color.gray;
        
        // Fill
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(sliderObj.transform);
        fill.AddComponent<Image>().color = name.Contains("Health") ? Color.red : Color.blue;
        
        slider.fillRect = fill.GetComponent<RectTransform>();
        
        return sliderObj;
    }
    
    void CreateCombatPanel(GameObject parent)
    {
        GameObject panel = CreatePanel(parent, "CombatPanel", new Vector2(0, -300));
        
        CreateButton(panel, "AttackButton", new Vector2(-150, 0), "Attack");
        CreateButton(panel, "SkillButton", new Vector2(-50, 0), "Skill");
        CreateButton(panel, "DefendButton", new Vector2(50, 0), "Defend");
        CreateButton(panel, "UseItemButton", new Vector2(150, 0), "Use Item");
        
        panel.SetActive(false);
    }
    
    void CreateQuestPanel(GameObject parent)
    {
        GameObject panel = CreatePanel(parent, "QuestPanel", new Vector2(600, 0));
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 600);
        
        CreateText(panel, "QuestTitle", new Vector2(0, 250), "Quest Title");
        CreateText(panel, "QuestDescription", new Vector2(0, 150), "Quest Description");
        
        for (int i = 0; i < 4; i++)
        {
            CreateButton(panel, $"ChoiceButton{i}", new Vector2(0, 50 - i * 60), $"Choice {i + 1}");
        }
        
        panel.SetActive(false);
    }
    
    void CreateDialoguePanel(GameObject parent)
    {
        GameObject panel = CreatePanel(parent, "DialoguePanel", new Vector2(0, -200));
        
        CreateText(panel, "NPCName", new Vector2(0, 50), "NPC Name");
        CreateText(panel, "NPCDialogue", new Vector2(0, 0), "NPC Dialogue");
        CreateButton(panel, "ContinueButton", new Vector2(0, -50), "Continue");
        
        panel.SetActive(false);
    }
    
    void CreateOutcomePanel(GameObject parent)
    {
        GameObject panel = CreatePanel(parent, "OutcomePanel", new Vector2(0, 0));
        
        CreateText(panel, "OutcomeText", new Vector2(0, 50), "Outcome Text");
        CreateButton(panel, "OutcomeOkButton", new Vector2(0, -50), "OK");
        
        panel.SetActive(false);
    }
    
    GameObject CreatePanel(GameObject parent, string name, Vector2 position)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent.transform);
        
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(300, 200);
        
        panel.AddComponent<Image>().color = new Color(0, 0, 0, 0.8f);
        
        return panel;
    }
    
    GameObject CreateButton(GameObject parent, string name, Vector2 position, string text)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent.transform);
        
        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(120, 40);
        
        buttonObj.AddComponent<Image>().color = Color.white;
        buttonObj.AddComponent<Button>();
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform);
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.black;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        return buttonObj;
    }
    
    GameObject CreateText(GameObject parent, string name, Vector2 position, string text)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform);
        
        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(280, 50);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        
        return textObj;
    }
    
    void ConnectReferences()
    {
        // Find all managers
        GameManager gameManager = FindObjectOfType<GameManager>();
        GridManager gridManager = FindObjectOfType<GridManager>();
        TurnManager turnManager = FindObjectOfType<TurnManager>();
        QuestManager questManager = FindObjectOfType<QuestManager>();
        UIManager uiManager = FindObjectOfType<UIManager>();
        LocalDataManager dataManager = FindObjectOfType<LocalDataManager>();
        
        // Connect GameManager references
        if (gameManager != null)
        {
            gameManager.gridManager = gridManager;
            gameManager.turnManager = turnManager;
            gameManager.questManager = questManager;
            gameManager.uiManager = uiManager;
            gameManager.dataManager = dataManager;
        }
        
        // Connect UI references
        if (uiManager != null)
        {
            uiManager.healthBar = GameObject.Find("HealthBar")?.GetComponent<Slider>();
            uiManager.manaBar = GameObject.Find("ManaBar")?.GetComponent<Slider>();
            uiManager.combatPanel = GameObject.Find("CombatPanel");
            uiManager.questPanel = GameObject.Find("QuestPanel");
            uiManager.dialoguePanel = GameObject.Find("DialoguePanel");
            uiManager.outcomePanel = GameObject.Find("OutcomePanel");
            
            // Connect buttons
            uiManager.attackButton = GameObject.Find("AttackButton")?.GetComponent<Button>();
            uiManager.skillButton = GameObject.Find("SkillButton")?.GetComponent<Button>();
            uiManager.defendButton = GameObject.Find("DefendButton")?.GetComponent<Button>();
            uiManager.useItemButton = GameObject.Find("UseItemButton")?.GetComponent<Button>();
            
            // Connect choice buttons
            uiManager.choiceButtons = new Button[4];
            for (int i = 0; i < 4; i++)
            {
                uiManager.choiceButtons[i] = GameObject.Find($"ChoiceButton{i}")?.GetComponent<Button>();
            }
        }
        
        Debug.Log("All references connected!");
    }
}