# Echoes of Elder Dawn - Unity Setup Guide

## Project Setup Instructions

### 1. Unity Project Creation
- Create new Unity 2D project
- Import TextMeshPro (Window > TextMeshPro > Import TMP Essential Resources)
- Set up project structure as created in Assets/Scripts/

### 2. Scene Setup

#### Main Camera
- Position: (0, 0, -10)
- Size: 5 (for 2D orthographic)

#### Grid System
1. Create GameObject "Grid"
2. Add Grid component
3. Add child GameObject "Tilemap"
4. Add TilemapRenderer and Tilemap components to child
5. Assign to GridManager script

#### Player Setup
1. Create GameObject "Player"
2. Add SpriteRenderer (assign player sprite)
3. Add PlayerController script
4. Add Collider2D
5. Position at (5, 5, 0)

### 3. UI Setup

#### Canvas Setup
1. Create UI Canvas (Screen Space - Overlay)
2. Add CanvasScaler (Scale With Screen Size, Reference Resolution: 1920x1080)

#### Health/Mana Bars
1. Create UI > Slider for Health Bar
2. Create UI > Slider for Mana Bar
3. Add TextMeshPro components for health/mana text
4. Position in top-left corner

#### Combat Panel
1. Create UI > Panel "CombatPanel"
2. Add 4 buttons: Attack, Skill, Defend, Use Item
3. Position in bottom-center
4. Set inactive by default

#### Quest Panel
1. Create UI > Panel "QuestPanel"
2. Add TextMeshPro for title and description
3. Add 4 choice buttons
4. Position on right side
5. Set inactive by default

#### Dialogue Panel
1. Create UI > Panel "DialoguePanel"
2. Add TextMeshPro for NPC name and dialogue
3. Add Continue button
4. Position in bottom-center
5. Set inactive by default

### 4. Manager Setup

#### GameManager GameObject
1. Create empty GameObject "GameManager"
2. Add GameManager script
3. Assign all manager references:
   - GridManager
   - TurnManager
   - QuestManager
   - UIManager
   - LocalDataManager

#### Individual Managers
1. Create empty GameObjects for each manager
2. Add respective scripts
3. Assign UI references in UIManager

### 5. Prefabs to Create

#### Damage Text Prefab
1. Create UI > TextMeshPro
2. Set color to red
3. Set font size to 24
4. Save as prefab "DamageText"
5. Assign to UIManager

### 6. Tags Setup
- Create tags: "NPC", "Interactable", "Enemy"

### 7. Input Setup
- Ensure Input Manager has WASD and Arrow keys configured
- Or use new Input System if preferred

### 8. Testing
1. Play scene
2. Use WASD to move
3. Press Q to open quest menu
4. Press E to interact
5. Combat should trigger randomly or at position (8,8)

## Script Assignment Checklist

- [ ] GameManager on GameManager GameObject
- [ ] GridManager on Grid GameObject  
- [ ] PlayerController on Player GameObject
- [ ] TurnManager on TurnManager GameObject
- [ ] QuestManager on QuestManager GameObject
- [ ] UIManager on UIManager GameObject
- [ ] LocalDataManager on DataManager GameObject
- [ ] LocalAIGenerator on AIGenerator GameObject

## Build Settings
- Add main scene to build settings
- Set platform to PC/Mac/Linux Standalone
- Configure player settings (company name, product name, etc.)

## Demo Flow
1. Player spawns in Solendra
2. Move around with WASD
3. Go to position (8,8) to trigger ruins quest
4. Experience combat system
5. Make quest choices
6. See AI-generated follow-up quests

## Future AWS Integration Points
- Replace LocalAIGenerator with AWS Bedrock calls
- Replace LocalDataManager with DynamoDB integration
- Add multiplayer sync endpoints
- Implement real-time AI quest generation

The game is fully playable locally and ready for demo!