using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public bool isMoving = false;
    
    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;
    public KeyCode questKey = KeyCode.Q;
    
    private Vector2Int currentGridPosition;
    private Vector3 targetWorldPosition;
    private GridManager gridManager;
    
    void Start()
    {
        gridManager = GameManager.Instance.gridManager;
        
        // Set initial position
        if (GameManager.Instance.currentPlayer != null)
        {
            currentGridPosition = GameManager.Instance.currentPlayer.gridPosition;
            transform.position = gridManager.GridToWorldPosition(currentGridPosition);
            targetWorldPosition = transform.position;
        }
    }
    
    void Update()
    {
        HandleInput();
        HandleMovement();
    }
    
    void HandleInput()
    {
        if (isMoving || GameManager.Instance.turnManager.inCombat) return;
        
        Vector2Int moveDirection = Vector2Int.zero;
        
        // WASD movement
        if (Input.GetKeyDown(KeyCode.W)) moveDirection = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S)) moveDirection = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A)) moveDirection = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D)) moveDirection = Vector2Int.right;
        
        // Arrow key movement
        if (Input.GetKeyDown(KeyCode.UpArrow)) moveDirection = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) moveDirection = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) moveDirection = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) moveDirection = Vector2Int.right;
        
        if (moveDirection != Vector2Int.zero)
        {
            TryMove(moveDirection);
        }
        
        // Interaction
        if (Input.GetKeyDown(interactKey))
        {
            CheckForInteractions();
        }
        
        // Quest menu
        if (Input.GetKeyDown(questKey))
        {
            ToggleQuestMenu();
        }
    }
    
    void TryMove(Vector2Int direction)
    {
        Vector2Int newGridPosition = currentGridPosition + direction;
        
        if (gridManager.CanMoveTo(currentGridPosition, newGridPosition))
        {
            StartMove(newGridPosition);
        }
        else
        {
            Debug.Log("Cannot move there!");
        }
    }
    
    void StartMove(Vector2Int newGridPosition)
    {
        currentGridPosition = newGridPosition;
        targetWorldPosition = gridManager.GridToWorldPosition(currentGridPosition);
        isMoving = true;
        
        // Update player data
        GameManager.Instance.currentPlayer.gridPosition = currentGridPosition;
        
        // Check for encounters
        CheckForEncounters();
    }
    
    void HandleMovement()
    {
        if (!isMoving) return;
        
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPosition, moveSpeed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, targetWorldPosition) < 0.1f)
        {
            transform.position = targetWorldPosition;
            isMoving = false;
            
            // Save position
            GameManager.Instance.SaveGame();
        }
    }
    
    void CheckForEncounters()
    {
        // Random encounter chance (10%)
        if (Random.Range(0f, 1f) < 0.1f)
        {
            TriggerRandomEncounter();
        }
        
        // Check for special locations
        CheckSpecialLocations();
    }
    
    void TriggerRandomEncounter()
    {
        Debug.Log("Random encounter!");
        
        // Create random enemy
        GameObject enemyObj = new GameObject("Wild Creature");
        CombatEntity enemy = enemyObj.AddComponent<CombatEntity>();
        enemy.Initialize("Wild Creature", Random.Range(30, 50), Random.Range(30, 50));
        
        var combatants = new System.Collections.Generic.List<CombatEntity>
        {
            GameManager.Instance.currentPlayer,
            enemy
        };
        
        GameManager.Instance.turnManager.StartCombat(combatants);
    }
    
    void CheckSpecialLocations()
    {
        // Check if player is at ruins location (example: position 8,8)
        if (currentGridPosition == new Vector2Int(8, 8))
        {
            Debug.Log("You've discovered ancient ruins!");
            GameManager.Instance.questManager.LoadQuest("sigil_of_eldara");
        }
    }
    
    void CheckForInteractions()
    {
        // Check for NPCs or interactable objects nearby
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        
        foreach (Collider2D col in nearby)
        {
            if (col.CompareTag("NPC"))
            {
                InteractWithNPC(col.gameObject);
                break;
            }
            else if (col.CompareTag("Interactable"))
            {
                InteractWithObject(col.gameObject);
                break;
            }
        }
    }
    
    void InteractWithNPC(GameObject npc)
    {
        Debug.Log($"Talking to {npc.name}");
        
        // Example NPC interaction
        if (npc.name.Contains("Elder"))
        {
            GameManager.Instance.uiManager.ShowNPCDialogue(
                "Village Elder", 
                "Welcome, brave adventurer! The ancient ruins hold many secrets..."
            );
        }
    }
    
    void InteractWithObject(GameObject obj)
    {
        Debug.Log($"Interacting with {obj.name}");
        
        // Example object interaction
        if (obj.name.Contains("Chest"))
        {
            // Give player a health potion
            GameManager.Instance.currentPlayer.Heal(20);
            GameManager.Instance.uiManager.ShowQuestOutcome("You found a health potion and restored 20 HP!");
        }
    }
    
    void ToggleQuestMenu()
    {
        bool isActive = GameManager.Instance.uiManager.questPanel.activeInHierarchy;
        GameManager.Instance.uiManager.ShowQuestUI(!isActive);
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw interaction range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCircle(transform.position, 1.5f);
        
        // Draw current grid position
        if (gridManager != null)
        {
            Gizmos.color = Color.red;
            Vector3 gridCenter = gridManager.GridToWorldPosition(currentGridPosition);
            Gizmos.DrawWireCube(gridCenter, Vector3.one);
        }
    }
}