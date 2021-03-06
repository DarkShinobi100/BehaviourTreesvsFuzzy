﻿using UnityEngine;

public class NewGame : MonoBehaviour
{
    [SerializeField]
    private Animator stateMachine;
    [SerializeField]
    private EnemyBehaviorTree enemyBehaviorTreeFar;
    [SerializeField]
    private EnemyBehaviorTree enemyBehaviorTreeNear;
    [SerializeField]
    private Player humanPlayer;
    [SerializeField]
    private Player aiPlayer;
    [SerializeField]
    private UIController uiController;
    private int turn = 0;

    private void Awake()
    {
        enemyBehaviorTreeFar.SetPlayerData(humanPlayer, aiPlayer);
        enemyBehaviorTreeFar.onTreeExecuted += EndTurn;
        enemyBehaviorTreeNear.SetPlayerData(aiPlayer, humanPlayer);
        enemyBehaviorTreeNear.onTreeExecuted += EndTurn;
    }

    public void EvaluateAITree()
    {
        enemyBehaviorTreeFar.Evaluate();
    }

    public void EvaluateAITree2()
    {
        enemyBehaviorTreeNear.Evaluate();
    }

    private void EndTurn()
    {
        if (humanPlayer.CurrentHealth <= 0 || aiPlayer.CurrentHealth <= 0)
        {
            stateMachine.SetTrigger("EndGame");
            uiController.EndGame();
            return;
        }
        stateMachine.SetTrigger("EndTurn");
        turn ^= 1;
        uiController.SetTurn(turn);
    }
}
