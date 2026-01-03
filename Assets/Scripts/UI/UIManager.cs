using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Health & Mana")]
    public Slider healthBar;
    public Slider manaBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    
    [Header("Combat UI")]
    public GameObject combatPanel;
    public Button attackButton;
    public Button skillButton;
    public Button defendButton;
    public Button useItemButton;
    
    [Header("Quest UI")]
    public GameObject questPanel;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;
    public Button[] choiceButtons;
    public TextMeshProUGUI[] choiceTexts;
    
    [Header("NPC Dialogue")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogue;
    public Button continueButton;
    
    [Header("Damage Effects")]
    public GameObject damageTextPrefab;
    public Transform damageParent;
    
    [Header("Outcome Display")]
    public GameObject outcomePanel;
    public TextMeshProUGUI outcomeText;
    public Button outcomeOkButton;
    
    void Start()
    {
        SetupButtons();
        ShowCombatUI(false);
        ShowQuestUI(false);
        ShowDialogueUI(false);
        ShowOutcomeUI(false);
    }
    
    void SetupButtons()
    {
        attackButton.onClick.AddListener(() => OnCombatAction(CombatAction.Attack));
        skillButton.onClick.AddListener(() => OnCombatAction(CombatAction.Skill));
        defendButton.onClick.AddListener(() => OnCombatAction(CombatAction.Defend));
        useItemButton.onClick.AddListener(() => OnCombatAction(CombatAction.UseItem));
        
        continueButton.onClick.AddListener(OnDialogueContinue);
        outcomeOkButton.onClick.AddListener(() => ShowOutcomeUI(false));
        
        // Setup choice buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i; // Capture for closure
            choiceButtons[i].onClick.AddListener(() => OnQuestChoice(index));
        }
    }
    
    public void UpdateUI()
    {
        Player player = GameManager.Instance.currentPlayer;
        if (player == null) return;
        
        // Update health bar
        healthBar.value = (float)player.health / player.maxHealth;
        healthText.text = $"{player.health}/{player.maxHealth}";
        
        // Update mana bar
        manaBar.value = (float)player.mana / player.maxMana;
        manaText.text = $"{player.mana}/{player.maxMana}";
        
        // Update skill button availability
        skillButton.interactable = player.mana >= 10;
    }
    
    public void ShowCombatUI(bool show)
    {
        combatPanel.SetActive(show);
        if (show) UpdateUI();
    }
    
    public void ShowQuestUI(bool show)
    {
        questPanel.SetActive(show);
    }
    
    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
    }
    
    public void ShowOutcomeUI(bool show)
    {
        outcomePanel.SetActive(show);
    }
    
    public void UpdateQuestUI(QuestTemplate quest)
    {
        questTitle.text = quest.title;
        questDescription.text = quest.description;
        
        // Update choice buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < quest.choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceTexts[i].text = quest.choices[i];
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
        
        ShowQuestUI(true);
        
        // Show NPC dialogue first
        ShowNPCDialogue(quest.npcName, quest.npcDialogue);
    }
    
    public void ShowNPCDialogue(string name, string dialogue)
    {
        npcName.text = name;
        npcDialogue.text = dialogue;
        ShowDialogueUI(true);
    }
    
    public void ShowQuestOutcome(string outcome)
    {
        outcomeText.text = outcome;
        ShowOutcomeUI(true);
    }
    
    public void ShowQuestComplete(string questTitle)
    {
        outcomeText.text = $"Quest Complete: {questTitle}\n\nYou have been fully healed!";
        ShowOutcomeUI(true);
    }
    
    public void ShowDamageEffect(Vector3 worldPosition, int damage)
    {
        if (damageTextPrefab == null) return;
        
        // Convert world position to screen position
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        
        GameObject damageObj = Instantiate(damageTextPrefab, damageParent);
        damageObj.transform.position = screenPos;
        
        TextMeshProUGUI damageText = damageObj.GetComponent<TextMeshProUGUI>();
        damageText.text = damage.ToString();
        
        // Animate damage text
        StartCoroutine(AnimateDamageText(damageObj));
    }
    
    IEnumerator AnimateDamageText(GameObject damageObj)
    {
        float duration = 1.5f;
        Vector3 startPos = damageObj.transform.position;
        Vector3 endPos = startPos + Vector3.up * 50;
        
        TextMeshProUGUI text = damageObj.GetComponent<TextMeshProUGUI>();
        Color startColor = text.color;
        
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            
            damageObj.transform.position = Vector3.Lerp(startPos, endPos, progress);
            text.color = Color.Lerp(startColor, Color.clear, progress);
            
            yield return null;
        }
        
        Destroy(damageObj);
    }
    
    void OnCombatAction(CombatAction action)
    {
        GameManager.Instance.turnManager.ExecutePlayerAction(action);
    }
    
    void OnQuestChoice(int choiceIndex)
    {
        GameManager.Instance.questManager.MakeChoice(choiceIndex);
        ShowQuestUI(false);
    }
    
    void OnDialogueContinue()
    {
        ShowDialogueUI(false);
    }
    
    void Update()
    {
        // Update UI every frame
        if (GameManager.Instance?.currentPlayer != null)
        {
            UpdateUI();
        }
    }
}