using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class QuestTemplate
{
    public string id;
    public string title;
    public string description;
    public string[] choices;
    public string[] outcomes;
    public string npcName;
    public string npcDialogue;
}

public class QuestManager : MonoBehaviour
{
    [Header("Current Quest")]
    public QuestTemplate currentQuest;
    public int currentStep = 0;
    
    [Header("Quest Database")]
    public Dictionary<string, QuestTemplate> questDatabase;
    
    void Start()
    {
        InitializeQuests();
    }
    
    void InitializeQuests()
    {
        questDatabase = new Dictionary<string, QuestTemplate>
        {
            ["sigil_of_eldara"] = new QuestTemplate
            {
                id = "sigil_of_eldara",
                title = "Recover the Sigil of Eldara",
                description = "The ancient ruins of Zlatne Rayhi Aeritha hold the legendary Sigil of Eldara. The Village Elder believes it can restore balance to Solendra.",
                choices = new[] {
                    "Sneak into ruins",
                    "Charge boldly",
                    "Negotiate with them",
                    "Seek village wisdom"
                },
                outcomes = new[] {
                    "You discover a hidden passage and avoid the guardians.",
                    "You fight through the ancient guardians protecting the ruins.",
                    "You attempt to communicate with the mystical beings.",
                    "You return to gather more information from the village elder."
                },
                npcName = "Village Elder",
                npcDialogue = "The Sigil of Eldara lies within the golden ruins. Choose your path wisely, young adventurer."
            },
            
            ["shadow_threat"] = new QuestTemplate
            {
                id = "shadow_threat",
                title = "Shadows from Nox'Varyn",
                description = "Dark creatures from the eastern lands threaten Solendra. Investigate their source.",
                choices = new[] {
                    "Track the shadows",
                    "Fortify the village"
                },
                outcomes = new[] {
                    "You follow the shadow trail toward the dark lands of Nox'Varyn.",
                    "You help the villagers prepare defenses against the coming darkness."
                },
                npcName = "Guard Captain",
                npcDialogue = "These shadows are unlike anything we've seen. They come from the cursed lands to the east."
            }
        };
    }
    
    public void LoadQuest(string questId)
    {
        if (questDatabase.ContainsKey(questId))
        {
            currentQuest = questDatabase[questId];
            currentStep = 0;
            
            GameManager.Instance.uiManager.UpdateQuestUI(currentQuest);
            Debug.Log($"Quest Loaded: {currentQuest.title}");
        }
    }
    
    public void MakeChoice(int choiceIndex)
    {
        if (currentQuest == null || choiceIndex >= currentQuest.choices.Length)
            return;
            
        string choice = currentQuest.choices[choiceIndex];
        string outcome = currentQuest.outcomes[choiceIndex];
        
        Debug.Log($"Player chose: {choice}");
        Debug.Log($"Outcome: {outcome}");
        
        // Process choice consequences
        ProcessChoiceConsequence(choiceIndex);
        
        // Show outcome to player
        GameManager.Instance.uiManager.ShowQuestOutcome(outcome);
        
        // Progress quest or start new one
        ProgressQuest(choiceIndex);
    }
    
    void ProcessChoiceConsequence(int choiceIndex)
    {
        Player player = GameManager.Instance.currentPlayer;
        
        switch (currentQuest.id)
        {
            case "sigil_of_eldara":
                switch (choiceIndex)
                {
                    case 0: // Sneak
                        // No combat, but less reward
                        CompleteQuest();
                        break;
                    case 1: // Charge
                        // Start combat
                        StartRuinsGuardianCombat();
                        break;
                    case 2: // Negotiate
                        // Skill check - restore some mana
                        player.mana = Mathf.Min(player.mana + 20, player.maxMana);
                        CompleteQuest();
                        break;
                    case 3: // Seek wisdom
                        // Get hint for next quest
                        LoadQuest("shadow_threat");
                        break;
                }
                break;
        }
    }
    
    void StartRuinsGuardianCombat()
    {
        // Create enemy for combat
        GameObject enemyObj = new GameObject("Ruins Guardian");
        CombatEntity enemy = enemyObj.AddComponent<CombatEntity>();
        enemy.Initialize("Ruins Guardian", 60, 60);
        
        List<CombatEntity> combatants = new List<CombatEntity>
        {
            GameManager.Instance.currentPlayer,
            enemy
        };
        
        GameManager.Instance.turnManager.StartCombat(combatants);
    }
    
    public void OnCombatWon()
    {
        if (currentQuest.id == "sigil_of_eldara")
        {
            CompleteQuest();
        }
    }
    
    void CompleteQuest()
    {
        Debug.Log($"Quest Completed: {currentQuest.title}");
        
        // Reward player
        Player player = GameManager.Instance.currentPlayer;
        player.health = player.maxHealth; // Full heal
        player.mana = player.maxMana; // Full mana
        
        GameManager.Instance.uiManager.ShowQuestComplete(currentQuest.title);
        
        // Load next quest or end demo
        LoadQuest("shadow_threat");
    }
    
    void ProgressQuest(int choiceIndex)
    {
        currentStep++;
        GameManager.Instance.SaveGame();
    }
    
    // Simulate AI-generated quest (for demo purposes)
    public QuestTemplate GenerateAIQuest(string context)
    {
        // This simulates what Bedrock would return
        return new QuestTemplate
        {
            id = "ai_generated",
            title = "[AI Generated] Mystery of the Ancient Grove",
            description = $"Based on your actions in {context}, strange magical energies have awakened in the forest...",
            choices = new[] {
                "Investigate the magical disturbance",
                "Consult the forest spirits"
            },
            outcomes = new[] {
                "You discover the source of the magical anomaly.",
                "The spirits reveal ancient secrets of Elarion."
            },
            npcName = "Forest Spirit",
            npcDialogue = "The balance of nature has been disturbed, mortal. Will you help restore it?"
        };
    }
}