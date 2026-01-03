using UnityEngine;

[System.Serializable]
public class GameState
{
    public Player player;
    public string currentQuest;
    public int questStep;
    public bool[] completedQuests;
    public string currentRegion = "Solendra";
    
    public GameState()
    {
        completedQuests = new bool[10]; // Support for 10 quests
    }
}

[System.Serializable]
public class Player : CombatEntity
{
    public int maxHealth = 100;
    public int maxMana = 50;
    public Vector2Int gridPosition;
    public bool isDefending = false;
    
    public Player()
    {
        name = "Player";
        health = maxHealth;
        mana = maxMana;
    }
}

[System.Serializable]
public class CombatEntity : MonoBehaviour
{
    public string entityName;
    public int health;
    public int maxHealth;
    public int mana;
    public bool isDefending = false;
    
    public virtual void Initialize(string name, int hp, int maxHp)
    {
        entityName = name;
        health = hp;
        maxHealth = maxHp;
        mana = 0;
    }
    
    public bool IsAlive()
    {
        return health > 0;
    }
    
    public void TakeDamage(int damage)
    {
        if (isDefending)
        {
            damage = damage / 2; // 50% reduction when defending
            isDefending = false;
        }
        
        health -= damage;
        health = Mathf.Max(0, health);
    }
    
    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Min(health, maxHealth);
    }
}

[System.Serializable]
public class QuestData
{
    public string id;
    public string title;
    public string description;
    public string[] choices;
    public string[] outcomes;
    public bool isCompleted;
    public int currentStep;
}

public enum Region
{
    Solendra,
    NoxVaryn,
    Aurelis
}

[System.Serializable]
public class WorldState
{
    public Region currentRegion = Region.Solendra;
    public bool[] regionEvents;
    public string[] regionDescriptions = {
        "The lush mountains of Solendra, where ancient magic flows through golden ruins.",
        "The dark lands of Nox'Varyn, shrouded in shadow and mystery.",
        "The sun-blessed deserts of Aurelis, where lost civilizations sleep beneath the sands."
    };
    
    public WorldState()
    {
        regionEvents = new bool[10]; // Track events across regions
    }
    
    public string GetRegionDescription(Region region)
    {
        return regionDescriptions[(int)region];
    }
}