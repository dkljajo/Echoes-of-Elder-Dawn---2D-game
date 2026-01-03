using UnityEngine;
using System.Collections.Generic;

public class LocalAIGenerator : MonoBehaviour
{
    [Header("AI Templates")]
    public List<QuestTemplate> questTemplates;
    public List<string> npcNames;
    public List<string> locationNames;
    
    [Header("Context")]
    public string currentRegion = "Solendra";
    public string playerActions = "";
    
    void Start()
    {
        InitializeTemplates();
    }
    
    void InitializeTemplates()
    {
        questTemplates = new List<QuestTemplate>();
        npcNames = new List<string> { "Village Elder", "Forest Spirit", "Ancient Guardian", "Mysterious Traveler", "Guard Captain" };
        locationNames = new List<string> { "Golden Ruins", "Dark Forest", "Crystal Cave", "Ancient Temple", "Mystic Grove" };
        
        // Create base templates that simulate AI generation
        CreateQuestTemplates();
    }
    
    void CreateQuestTemplates()
    {
        questTemplates.Add(new QuestTemplate
        {
            id = "ai_mystery_grove",
            title = "[AI] Mystery of the Enchanted Grove",
            description = "Strange magical energies emanate from the ancient grove. The trees whisper of forgotten secrets.",
            choices = new[] {
                "Investigate the magical source",
                "Commune with the forest spirits",
                "Search for ancient artifacts",
                "Seek guidance from the druids"
            },
            outcomes = new[] {
                "You discover a hidden shrine pulsing with elder magic.",
                "The spirits reveal the location of a powerful artifact.",
                "You uncover relics from the age of Elder Dawn.",
                "The druids share ancient wisdom about the realm's history."
            },
            npcName = "Forest Spirit",
            npcDialogue = "The grove remembers the time before darkness fell upon Nox'Varyn. Will you help restore the balance?"
        });
        
        questTemplates.Add(new QuestTemplate
        {
            id = "ai_shadow_incursion",
            title = "[AI] Shadow Incursion from the East",
            description = "Dark creatures from Nox'Varyn have crossed into Solendra. Their presence corrupts the land.",
            choices = new[] {
                "Track the shadow creatures to their source",
                "Rally the village defenders",
                "Seek magical protection",
                "Investigate the corruption's cause"
            },
            outcomes = new[] {
                "You follow a dark trail leading toward the cursed eastern lands.",
                "The villagers unite to form a protective barrier.",
                "You acquire a blessed amulet that wards off shadow magic.",
                "You discover the shadows are fleeing from something even darker."
            },
            npcName = "Guard Captain",
            npcDialogue = "These shadows are unlike any we've faced. They seem... afraid. What could frighten creatures of pure darkness?"
        });
        
        questTemplates.Add(new QuestTemplate
        {
            id = "ai_aurelis_expedition",
            title = "[AI] The Lost Expedition to Aurelis",
            description = "A research expedition to the southern deserts of Aurelis has gone missing. Their last message spoke of a great discovery.",
            choices = new[] {
                "Follow their trail into the desert",
                "Consult the desert nomads",
                "Search ancient maps for clues",
                "Prepare for a dangerous journey"
            },
            outcomes = new[] {
                "You find traces of the expedition near a buried temple.",
                "The nomads speak of strange lights in the deep desert.",
                "Old maps reveal the location of a lost city.",
                "You gather supplies and allies for the perilous quest ahead."
            },
            npcName = "Desert Scholar",
            npcDialogue = "The sands of Aurelis hold many secrets. The expedition sought the Crown of Solar Winds... but some treasures are better left buried."
        });
    }
    
    public QuestTemplate GenerateContextualQuest(string context, string playerChoice = "")
    {
        // Simulate AI decision-making based on context
        QuestTemplate selectedQuest = null;
        
        // Simple context-based selection (simulates AI reasoning)
        if (context.Contains("forest") || context.Contains("nature"))
        {
            selectedQuest = questTemplates.Find(q => q.id == "ai_mystery_grove");
        }
        else if (context.Contains("shadow") || context.Contains("dark"))
        {
            selectedQuest = questTemplates.Find(q => q.id == "ai_shadow_incursion");
        }
        else if (context.Contains("desert") || context.Contains("south"))
        {
            selectedQuest = questTemplates.Find(q => q.id == "ai_aurelis_expedition");
        }
        else
        {
            // Random selection if no context match
            selectedQuest = questTemplates[Random.Range(0, questTemplates.Count)];
        }
        
        // Add AI generation flavor text
        if (selectedQuest != null)
        {
            selectedQuest.description += $"\n\n[Generated based on: {context}]";
        }
        
        return selectedQuest;
    }
    
    public string GenerateNPCDialogue(string npcType, string questContext)
    {
        // Simulate AI-generated dialogue based on NPC type and context
        Dictionary<string, string[]> dialogueTemplates = new Dictionary<string, string[]>
        {
            ["elder"] = new[] {
                "The ancient ways speak of great change coming to Elarion...",
                "In my years, I have seen the ebb and flow of magic across our lands...",
                "The signs are clear - the Elder Dawn stirs once more..."
            },
            ["spirit"] = new[] {
                "The natural world whispers of disturbances in the magical flow...",
                "Ancient powers awaken, both light and shadow...",
                "The balance must be maintained, mortal one..."
            },
            ["guard"] = new[] {
                "Strange reports come from all corners of the realm...",
                "Our patrols have encountered things that shouldn't exist...",
                "The people look to heroes like you for protection..."
            }
        };
        
        string[] templates = dialogueTemplates.ContainsKey(npcType.ToLower()) 
            ? dialogueTemplates[npcType.ToLower()] 
            : new[] { "Greetings, traveler. These are troubled times..." };
            
        string baseDialogue = templates[Random.Range(0, templates.Length)];
        
        return $"{baseDialogue}\n\n[Context: {questContext}]";
    }
    
    public QuestTemplate CreateDynamicQuest(string playerAction, string currentLocation)
    {
        // Simulate dynamic quest creation based on player actions
        string questId = $"dynamic_{Random.Range(1000, 9999)}";
        string title = $"[AI Dynamic] Consequences of {playerAction}";
        
        string description = $"Your decision to {playerAction} in {currentLocation} has had unexpected consequences. " +
                           $"The magical energies of Elarion respond to your choices...";
        
        string[] dynamicChoices = {
            $"Investigate the results of your {playerAction}",
            "Seek to undo any negative consequences",
            "Embrace the changes you've caused",
            "Consult with local experts about the situation"
        };
        
        string[] dynamicOutcomes = {
            $"Your investigation reveals the true impact of {playerAction} on the local area.",
            "You discover a way to mitigate the unintended effects of your actions.",
            "You decide to accept responsibility and work with the new circumstances.",
            "Local wisdom helps you understand the broader implications of your choice."
        };
        
        return new QuestTemplate
        {
            id = questId,
            title = title,
            description = description,
            choices = dynamicChoices,
            outcomes = dynamicOutcomes,
            npcName = GetRandomNPC(),
            npcDialogue = GenerateNPCDialogue("elder", $"player {playerAction} in {currentLocation}")
        };
    }
    
    string GetRandomNPC()
    {
        return npcNames[Random.Range(0, npcNames.Count)];
    }
    
    string GetRandomLocation()
    {
        return locationNames[Random.Range(0, locationNames.Count)];
    }
    
    // Public method for quest manager to use
    public QuestTemplate GetAIQuest(string context = "")
    {
        if (string.IsNullOrEmpty(context))
            context = $"exploring {currentRegion}";
            
        return GenerateContextualQuest(context);
    }
}