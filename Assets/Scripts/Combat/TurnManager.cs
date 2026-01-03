using UnityEngine;
using System.Collections.Generic;

public enum CombatAction
{
    Attack,
    Skill,
    Defend,
    UseItem
}

public class TurnManager : MonoBehaviour
{
    [Header("Combat Settings")]
    public bool inCombat = false;
    public int currentTurn = 0;
    public List<CombatEntity> combatants = new List<CombatEntity>();
    
    [Header("Combat Stats")]
    public int baseDamage = 20;
    public int skillDamage = 35;
    public int defendReduction = 50; // 50% damage reduction
    
    public void StartCombat(List<CombatEntity> entities)
    {
        inCombat = true;
        combatants = entities;
        currentTurn = 0;
        
        GameManager.Instance.uiManager.ShowCombatUI(true);
        Debug.Log("Combat Started!");
    }
    
    public void ExecutePlayerAction(CombatAction action)
    {
        if (!inCombat) return;
        
        Player player = GameManager.Instance.currentPlayer;
        CombatEntity enemy = GetCurrentEnemy();
        
        switch (action)
        {
            case CombatAction.Attack:
                PerformAttack(player, enemy, baseDamage);
                break;
                
            case CombatAction.Skill:
                if (player.mana >= 10)
                {
                    PerformAttack(player, enemy, skillDamage);
                    player.mana -= 10;
                }
                break;
                
            case CombatAction.Defend:
                player.isDefending = true;
                Debug.Log("Player is defending!");
                break;
                
            case CombatAction.UseItem:
                UseHealthPotion(player);
                break;
        }
        
        // Enemy turn
        if (enemy.health > 0)
        {
            EnemyTurn(enemy, player);
        }
        
        // Check combat end
        if (enemy.health <= 0)
        {
            EndCombat(true);
        }
        else if (player.health <= 0)
        {
            EndCombat(false);
        }
        
        GameManager.Instance.uiManager.UpdateUI();
    }
    
    void PerformAttack(CombatEntity attacker, CombatEntity target, int damage)
    {
        int finalDamage = damage;
        
        if (target is Player player && player.isDefending)
        {
            finalDamage = damage * (100 - defendReduction) / 100;
            player.isDefending = false;
        }
        
        target.health -= finalDamage;
        target.health = Mathf.Max(0, target.health);
        
        Debug.Log($"{attacker.name} attacks {target.name} for {finalDamage} damage!");
        
        // Show damage effect
        GameManager.Instance.uiManager.ShowDamageEffect(target.transform.position, finalDamage);
    }
    
    void EnemyTurn(CombatEntity enemy, Player player)
    {
        // Simple AI: 70% attack, 30% defend
        if (Random.Range(0f, 1f) < 0.7f)
        {
            PerformAttack(enemy, player, 15);
        }
        else
        {
            enemy.isDefending = true;
            Debug.Log($"{enemy.name} is defending!");
        }
    }
    
    void UseHealthPotion(Player player)
    {
        int healAmount = 30;
        player.health += healAmount;
        player.health = Mathf.Min(player.health, player.maxHealth);
        
        Debug.Log($"Player healed for {healAmount} HP!");
    }
    
    CombatEntity GetCurrentEnemy()
    {
        return combatants.Find(c => c != GameManager.Instance.currentPlayer);
    }
    
    void EndCombat(bool playerWon)
    {
        inCombat = false;
        combatants.Clear();
        
        GameManager.Instance.uiManager.ShowCombatUI(false);
        
        if (playerWon)
        {
            Debug.Log("Combat Won!");
            GameManager.Instance.questManager.OnCombatWon();
        }
        else
        {
            Debug.Log("Combat Lost!");
            // Respawn player
            GameManager.Instance.currentPlayer.health = GameManager.Instance.currentPlayer.maxHealth;
        }
    }
}