﻿using UnityEngine;
using UnityEngine.UI;

public class NewUI : MonoBehaviour
{
    private const string playerTurnMessage = "Your turn";
    private const string aiTurnMessage = "Enemy's turn";
    private const string gameOverMessage = "GAME OVER";
    private const string ActionMessage = "Now Thinking";
    private const string WaitMessage = "";
    private const string WinMessage = "You win!";
    private const string LoseMessage = "You Lose!";
    [SerializeField]
    private NewPlayer NearAIPlayer;
    [SerializeField]
    private NewPlayer FarAiPlayer;
    [SerializeField]
    private UpdatedEnemyBehaviorTree enemyBehaviorTreeFar;
    [SerializeField]
    private UpdatedEnemyBehaviorTree enemyBehaviorTreeNear;
    [SerializeField]
    private Text turnText;
    [SerializeField]
    private Text playerHealthText;
    [SerializeField]
    private Text enemyHealthText;

    //user sliders
    [SerializeField]
    private Slider NearHealth;
    [SerializeField]
    private Slider NearMana;
    [SerializeField]
    private Slider NearAttack;
    [SerializeField]
    private Slider NearDefence;
    [SerializeField]
    private Slider FarHealth;
    [SerializeField]
    private Slider FarMana;
    [SerializeField]
    private Slider FarAttack;
    [SerializeField]
    private Slider FarDefence;


    private void FixedUpdate()
    {
        UpdateSliders();
    }

    public void EndGame()
    {
        turnText.text = gameOverMessage;

        if (NearAIPlayer.CurrentHealth <= 0)
        {
            playerHealthText.text = LoseMessage;
            enemyHealthText.text = WinMessage;
        }
        else
        {
            playerHealthText.text = WinMessage;
            enemyHealthText.text = LoseMessage;
        }
    }

    /* We change the turn message dpending on whose turn it is currently */
    public void SetTurn(int turnNumber)
    {
        if (turnNumber == 0)
        {
            turnText.text = playerTurnMessage;
            playerHealthText.text = ActionMessage;
            enemyHealthText.text = WaitMessage;
            enemyBehaviorTreeFar.ResetSprites();
            enemyBehaviorTreeNear.UpdateSprites();
        }
        else
        {
            turnText.text = aiTurnMessage;
            enemyHealthText.text = ActionMessage;
            playerHealthText.text = WaitMessage;
            enemyBehaviorTreeNear.ResetSprites();
            enemyBehaviorTreeFar.UpdateSprites();
        }
    }

    private void UpdateSliders()
    {
        //Near AI
        NearHealth.value = NearAIPlayer.CurrentHealth;
        NearMana.value = NearAIPlayer.CurrentMana;
        NearAttack.value = NearAIPlayer.CurrentAttack;
        NearDefence.value = NearAIPlayer.CurrentDefence;
        //Far AI
        FarHealth.value = FarAiPlayer.CurrentHealth;
        FarMana.value = FarAiPlayer.CurrentMana;
        FarAttack.value = FarAiPlayer.CurrentAttack;
        FarDefence.value = FarAiPlayer.CurrentDefence;
    }
}
