using UnityEngine;

public class NewGame : MonoBehaviour
{
    [SerializeField]
    private Animator stateMachine;
    [SerializeField]
    private UpdatedEnemyBehaviorTree enemyBehaviorTreeFar;
    [SerializeField]
    private UpdatedEnemyBehaviorTree enemyBehaviorTreeNear;
    [SerializeField]
    bool Fuzzy = false;
    [SerializeField]
    private FuzzyBehaviourScript FuzzyenemyBehaviorTreeFar;
    [SerializeField]
    private FuzzyBehaviourScript FuzzyenemyBehaviorTreeNear;
    [SerializeField]
    private NewPlayer humanPlayer;
    [SerializeField]
    private NewPlayer aiPlayer;
    [SerializeField]
    private NewUI uiController;
    private int turn = 0;

    private void Awake()
    {
        if (!Fuzzy)
        {
            enemyBehaviorTreeFar.SetPlayerData(humanPlayer, aiPlayer);
            enemyBehaviorTreeFar.onTreeExecuted += EndTurn;
            enemyBehaviorTreeNear.SetPlayerData(aiPlayer, humanPlayer);
            enemyBehaviorTreeNear.onTreeExecuted += EndTurn;
        }
        else
        {
            FuzzyenemyBehaviorTreeFar.SetPlayerData(humanPlayer, aiPlayer);
            FuzzyenemyBehaviorTreeFar.onTreeExecuted += EndTurn;
            FuzzyenemyBehaviorTreeNear.SetPlayerData(aiPlayer, humanPlayer);
            FuzzyenemyBehaviorTreeNear.onTreeExecuted += EndTurn;
        }

    }

    public void EvaluateAITree()
    {
        if (!Fuzzy)
        {
            enemyBehaviorTreeFar.Evaluate();
        }
        else
        {
            FuzzyenemyBehaviorTreeFar.Evaluate();
        }
    }

    public void EvaluateAITree2()
    {
        if (!Fuzzy)
        {
            enemyBehaviorTreeNear.Evaluate();
        }
        else
        {
            FuzzyenemyBehaviorTreeNear.Evaluate();
        }
    }

    private void EndTurn()
    {
        if (humanPlayer.CurrentHealth <= 0 || aiPlayer.CurrentHealth <= 0)
        {
            stateMachine.SetTrigger("EndGame");
            uiController.EndGame();
            return;
        }
        if(!Fuzzy)
        {
            enemyBehaviorTreeFar.UpdateSprites();
            enemyBehaviorTreeNear.UpdateSprites();
        }
        else
        {
            FuzzyenemyBehaviorTreeFar.UpdateSprites();
            FuzzyenemyBehaviorTreeNear.UpdateSprites();
        }
        stateMachine.SetTrigger("EndTurn");
        turn ^= 1;
        uiController.SetTurn(turn);
    }
}
